using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Representations;
using System;
using System.Globalization;

namespace Byces.Calculator.Expressions
{
    internal sealed class ContentBuilder
    {
        public ContentBuilder(Content content)
        {
            _content = content;
        }
        
        internal int FirstIndex;

        internal int LastIndex;

        private int _priority;

        private bool _isNegative;

        private char _groupSeparator;

        private char _decimalSeparator;

        private NumberFormatInfo _numberFormatInfo = null!;

        private readonly Content _content;

        public void Clear()
        {
            FirstIndex = 0;
            LastIndex = 0;
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
                while (!reachedEnd && !ScanExpressionBeforeNumber(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
                while (!reachedEnd && !ScanExpressionAfterNumber(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
            } while (!reachedEnd);
            
            if (_priority > 0) throw new MissingParenthesesExpressionException();
            if (_content.Variables.Count != _content.Operations.Count + 1) throw new IncompleteExpressionException();
            if (LastIndex != FirstIndex) throw new UnknownNumberExpressionException();
        }
        
        private void IncreaseCycle(ReadOnlySpan<char> expressionSpan, out bool reachedEnd)
        {
            ++LastIndex;
            ++FirstIndex;
            reachedEnd = LastIndex >= expressionSpan.Length;
        }
        
        private bool ScanExpressionBeforeNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (LastIndex == FirstIndex && expressionSpan[LastIndex] == '(')
            {
                ++_priority;
                return false;
            }
            if (FindNumber(expressionSpan) || FindVariable(expressionSpan))
            {
                FirstIndex = LastIndex;
                return true;
            }
            if (FindFunction(expressionSpan))
            {
                FirstIndex = LastIndex;
                return false;
            }
            --FirstIndex;
            return false;
        }
        
        private bool ScanExpressionAfterNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (LastIndex == FirstIndex)
            {
                if (expressionSpan[LastIndex] == '(')
                {
                    AddOperator(OperatorRepresentation.Parse("*"));
                    ++_priority;
                    return true;
                }
                if (expressionSpan[LastIndex] == ')')
                {
                    if (--_priority < 0) throw new MisplacedParenthesesExpressionException();
                    return false;
                }
            }
            if (FindOperator(expressionSpan))
            {
                FirstIndex = LastIndex;
                return true;
            }
            --FirstIndex;
            return false;
        }
        
        private bool FindNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (FirstIndex != LastIndex) return false;
            
            _isNegative = expressionSpan[LastIndex] == '-';
            if (_isNegative || expressionSpan[LastIndex] == '+') { LastIndex++; FirstIndex++; }
            if (char.IsLetter(expressionSpan[LastIndex])) return false;

            var numberStyles = NumberStyles.None;
            for (bool atScientificNotation = false, atDecimal = false, hasSignal = false; LastIndex < expressionSpan.Length; LastIndex++)
            {
                char currentChar = expressionSpan[LastIndex];
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
            return ParseNumber(expressionSpan[FirstIndex..LastIndex], numberStyles);
        }
        
        private bool ParseNumber(ReadOnlySpan<char> currentSpan, NumberStyles numberStyles)
        {
            if (FirstIndex == LastIndex) return false;
            LastIndex--;
            
            if (!double.TryParse(currentSpan, numberStyles, _numberFormatInfo, out double result)) throw new UnknownNumberExpressionException();

            AddNumber(result);
            return true;
        }
        
        private bool FindVariable(ReadOnlySpan<char> expressionSpan)
        {
            VariableRepresentation? value = VariableRepresentation.FindVariable(this, expressionSpan);
            if (value == null) return false;

            AddVariable(value);
            return true;
        }
        
        private bool FindFunction(ReadOnlySpan<char> expressionSpan)
        {
            FunctionRepresentation? value = FunctionRepresentation.FindFunction(this, expressionSpan);
            if (value == null) return false;
            
            AddFunction(value);
            return true;
        }
        
        private bool FindOperator(ReadOnlySpan<char> expressionSpan)
        {
            OperatorRepresentation? value = OperatorRepresentation.FindOperator(this, expressionSpan);
            if (value == null) return false;

            AddOperator(value);
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