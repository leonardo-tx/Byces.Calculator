using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Representations;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Byces.Calculator.Expressions
{
    internal sealed class ContentBuilder
    {
        private static readonly OperatorRepresentation Multiplication = OperatorRepresentation.Parse("*");
        
        public ContentBuilder(Content content)
        {
            _content = content;
        }

        private int _firstIndex;

        private int _lastIndex;

        private int _priority;

        private bool _isNegative;

        private char _groupSeparator;

        private char _decimalSeparator;

        private NumberFormatInfo _numberFormatInfo = null!;

        private readonly Content _content;

        public void Clear()
        {
            _firstIndex = 0;
            _lastIndex = 0;
            _priority = 0;
            _isNegative = false;
        }

        public void Build(ReadOnlySpan<char> expressionSpan, CultureInfo cultureInfo)
        {
            _numberFormatInfo = cultureInfo.NumberFormat;
            _groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0];
            _decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0];
            ScanExpression(expressionSpan);
        }
        
        private void ScanExpression(ReadOnlySpan<char> expressionSpan)
        {
            bool reachedEnd = false;
            do
            {
                while (!reachedEnd && !ScanExpressionBeforeVariable(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
                while (!reachedEnd && !ScanExpressionAfterVariable(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
            } while (!reachedEnd);
            
            if (_priority > 0) throw new MissingParenthesesExpressionException();
            if (_content.Variables.Count != _content.Operations.Count + 1) throw new IncompleteExpressionException();
            if (_lastIndex != _firstIndex) throw new UnknownNumberExpressionException();
        }
        
        private void IncreaseCycle(ReadOnlySpan<char> expressionSpan, out bool reachedEnd)
        {
            ++_lastIndex;
            ++_firstIndex;
            reachedEnd = _lastIndex >= expressionSpan.Length;
        }
        
        private bool ScanExpressionBeforeVariable(ReadOnlySpan<char> expressionSpan)
        {
            if (expressionSpan[_lastIndex] == '(')
            {
                ++_priority;
                return false;
            }
            if (FindNumber(expressionSpan))
            {
                _firstIndex = _lastIndex;
                return true;
            }
            while (!FindVariableOrFunction(expressionSpan))
            {
                ++_lastIndex;
            }
            _firstIndex = _lastIndex;
            return _content.Variables.Count == _content.Operations.Count + 1;
        }
        
        private bool ScanExpressionAfterVariable(ReadOnlySpan<char> expressionSpan)
        {
            if (expressionSpan[_lastIndex] == '(')
            {
                AddOperator(Multiplication);
                ++_priority;
                return true;
            }
            if (expressionSpan[_lastIndex] == ')')
            {
                if (--_priority < 0) throw new MisplacedParenthesesExpressionException();
                return false;
            }
            while (!FindOperator(expressionSpan))
            {
                ++_lastIndex;
            }
            _firstIndex = _lastIndex;
            return true;
        }
        
        private bool FindNumber(ReadOnlySpan<char> expressionSpan)
        {
            _isNegative = expressionSpan[_lastIndex] == '-';
            if (_isNegative || expressionSpan[_lastIndex] == '+') { _lastIndex++; _firstIndex++; }
            if (char.IsLetter(expressionSpan[_lastIndex])) return false;

            var numberStyles = NumberStyles.None;
            for (bool atScientificNotation = false, atDecimal = false, hasSignal = false; _lastIndex < expressionSpan.Length; _lastIndex++)
            {
                char currentChar = expressionSpan[_lastIndex];
                if (char.IsDigit(currentChar)) continue;
                if (!atDecimal)
                {
                    if (currentChar == _decimalSeparator) { atDecimal = true; numberStyles |= NumberStyles.AllowDecimalPoint; continue; }
                    if (currentChar == _groupSeparator) { numberStyles |= NumberStyles.AllowThousands; continue; }
                }
                switch (atScientificNotation)
                {
                    case false when currentChar is 'E' or 'e':
                        atScientificNotation = true; numberStyles |= NumberStyles.AllowExponent; continue;
                    case true when !hasSignal && currentChar is '+' or '-':
                        hasSignal = true; continue;
                }
                break;
            }
            return ParseNumber(expressionSpan[_firstIndex.._lastIndex], numberStyles);
        }
        
        private bool ParseNumber(ReadOnlySpan<char> currentSpan, NumberStyles numberStyles)
        {
            if (currentSpan.IsEmpty) return false;
            _lastIndex--;
            
            if (!double.TryParse(currentSpan, numberStyles, _numberFormatInfo, out double result)) throw new UnknownNumberExpressionException();

            AddNumber(result);
            return true;
        }
        
        private bool FindOperator(ReadOnlySpan<char> expressionSpan)
        {
            if (_lastIndex == expressionSpan.Length) return true;
            if (!FindExpression(expressionSpan, out OperatorRepresentation? result)) return false;
            
            AddOperator(result);
            return true;
        }

        private bool FindVariableOrFunction(ReadOnlySpan<char> expressionSpan)
        {
            if (_lastIndex == expressionSpan.Length) return true;
            if (!FindExpression(expressionSpan, out BeforeVariableRepresentation? result)) return false;
            if (result is VariableRepresentation variableRepresentation2)
            {
                AddVariable(variableRepresentation2);
                return true;
            }
            AddFunction((FunctionRepresentation)result);
            return true;
        }

        private bool FindExpression<T>(ReadOnlySpan<char> expressionSpan, [NotNullWhen(true)] out T? result) where T : ExpressionRepresentation<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[_firstIndex..(_lastIndex + 1)];
            if (!ExpressionRepresentation<T>.TryParse(currentSpan, out result)) return false;
            
            int increaseLastIndex = 0, foundLength = currentSpan.Length;
            T? value = null;
            
            for (int i = 0; i < result.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = result.RepresentableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1;
                while (j <= representableConflict.Difference && _lastIndex + j < expressionSpan.Length) j++;
                
                currentSpan = expressionSpan[_firstIndex..(_lastIndex + --j + 1)];
                
                bool parseResult = ExpressionRepresentation<T>.TryParse(currentSpan, out T? value2);
                if (!parseResult || increaseLastIndex > j) continue;
                
                increaseLastIndex = j;
                value = value2;
            }
            if (value != null)
            {
                result = value;
                _lastIndex += increaseLastIndex;
            }
            return true;
        }
        
        private void AddFunction(FunctionRepresentation representation)
        {
            Function function = new(_content.Variables.Count, representation, _priority);
            _content.Functions.Add(function);
        }
        
        private void AddVariable(VariableRepresentation representation)
        {
            Variable value = representation.GetValue();
            if (_isNegative && value.Type == VariableType.Number)
            {
                _content.Variables.Add(-value.Number);
                return;
            }
            _content.Variables.Add(value);
        }
        
        private void AddNumber(double givenValue)
        {
            _content.Variables.Add(_isNegative ? -givenValue : givenValue);
        }
        
        private void AddOperator(OperatorRepresentation representation)
        {
            Operation operation = new(representation, _priority);
            _content.Operations.Add(operation);
            _content.UsedOperators |= representation.Priority;
        }
    }
}