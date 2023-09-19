using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Representations;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Byces.Calculator.Expressions
{
    internal sealed class ContentBuilder
    {
        public ContentBuilder(Content content)
        {
            _content = content;
        }
        
        private int _firstIndex;

        private int _lastIndex;

        private int _priority;

        private bool _afterNumber;

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
            _afterNumber = false;
            _isNegative = false;
        }

        public void Build(ReadOnlySpan<char> expressionSpan, CultureInfo cultureInfo)
        {
            _numberFormatInfo = cultureInfo.NumberFormat;
            _groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0];
            _decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0];
            ScanExpression(expressionSpan);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ScanExpression(ReadOnlySpan<char> expressionSpan)
        {
            for (; _lastIndex < expressionSpan.Length; _lastIndex++, _firstIndex++)
            {
                if (FindParentheses(expressionSpan[_lastIndex])) continue;
                if (!_afterNumber)
                {
                    if (FindNumber(expressionSpan) || FindVariable(expressionSpan))
                    {
                        _afterNumber = true; _firstIndex = _lastIndex;
                        continue;
                    }
                    if (FindFunction(expressionSpan)) { _firstIndex = _lastIndex; continue; }
                }
                else
                {
                    if (FindOperator(expressionSpan))
                    {
                        _firstIndex = _lastIndex; _afterNumber = false;
                        continue;
                    }
                }
                _firstIndex--;
            }
            if (_priority > 0) throw new MissingParenthesesExpressionException();
            if (!_afterNumber) throw new IncompleteExpressionException();
            if (_lastIndex != _firstIndex) throw new UnknownNumberExpressionException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindParentheses(char currentChar)
        {
            if (currentChar == '(')
            {
                if (_afterNumber)
                {
                    AddOperator(OperatorRepresentation.Parse("*"));
                    _afterNumber = false;
                }
                _priority++;
                return true;
            }
            if (currentChar != ')') return false;
            if (--_priority < 0) throw new MisplacedParenthesesExpressionException();
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (_firstIndex != _lastIndex) return false;
            
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ParseNumber(ReadOnlySpan<char> currentSpan, NumberStyles numberStyles)
        {
            if (_firstIndex == _lastIndex) return false;
            _lastIndex--;
            
            if (!double.TryParse(currentSpan, numberStyles, _numberFormatInfo, out double result)) throw new UnknownNumberExpressionException();

            AddNumber(result);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindVariable(ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<VariableRepresentation>(expressionSpan, ExpressionConflict.Variable, out bool isType);
            if (!isType || value == -1) return false;

            AddNumber(VariableRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindFunction(ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<FunctionRepresentation>(expressionSpan, ExpressionConflict.Function, out bool isType);
            if (!isType)
            {
                if (value != -1) _lastIndex--;
                return false;
            }
            AddFunction(FunctionRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindOperator(ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<OperatorRepresentation>(expressionSpan, ExpressionConflict.Operator, out _);
            if (value == -1) return false;

            AddOperator(OperatorRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValue<T>(ReadOnlySpan<char> expressionSpan, ExpressionConflict resultConflict, out bool isOriginalType) 
            where T : ExpressionRepresentation<T>
        {
            if (_lastIndex == expressionSpan.Length) { isOriginalType = false; return -1; }

            ReadOnlySpan<char> currentSpan = expressionSpan[_firstIndex..(_lastIndex + 1)];
            if (currentSpan.Length <= ExpressionRepresentation<T>.MaxStringSize &&
                ExpressionRepresentation<T>.TryParse(currentSpan, out T representation))
            {
                return FindConflicts(expressionSpan, representation, resultConflict, currentSpan.Length, out isOriginalType);
            }
            isOriginalType = false; return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindConflicts<T>(ReadOnlySpan<char> expressionSpan, T firstResult, ExpressionConflict resultConflict, int foundLength, out bool isOriginalType) 
            where T : ExpressionRepresentation<T>
        {
            int value = -1, increaseLastIndex = 0; isOriginalType = true;
            for (int i = 0; i < firstResult.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = firstResult.RepresentableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1;
                while (j <= representableConflict.Difference && _lastIndex + j < expressionSpan.Length) j++;
                
                ReadOnlySpan<char> currentSpan = expressionSpan[_firstIndex..(_lastIndex + --j + 1)];
                
                int value2 = GetGenericValue(currentSpan, representableConflict.ExpressionConflict);
                if (value2 == -1 || increaseLastIndex > j) continue;

                isOriginalType = representableConflict.ExpressionConflict == resultConflict;
                increaseLastIndex = j;
                value = value2;
            }
            if (value == -1) return firstResult.Value;
            
            _lastIndex += increaseLastIndex;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetGenericValue(ReadOnlySpan<char> currentSpan, ExpressionConflict conflict)
        {
            switch (conflict)
            {
                case ExpressionConflict.Operator:
                    if (OperatorRepresentation.TryParse(currentSpan, out var operatorRepresentation)) return operatorRepresentation.Value;
                    break;
                case ExpressionConflict.Function:
                    if (FunctionRepresentation.TryParse(currentSpan, out var functionRepresentation)) return functionRepresentation.Value;
                    break;
                case ExpressionConflict.Variable:
                    if (VariableRepresentation.TryParse(currentSpan, out var variableRepresentation)) return variableRepresentation.Value;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddFunction(FunctionRepresentation representation)
        {
            Function function = new(_content.Variables.Count, representation, _priority);
            _content.Functions.Add(function);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(VariableRepresentation representation)
        {
            Variable value = representation.GetValue();
            if (_isNegative && value.Type == VariableType.Number)
            {
                _content.Variables.Add(-value.Number);
                return;
            }
            _content.Variables.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(double givenValue)
        {
            _content.Variables.Add(_isNegative ? -givenValue : givenValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddOperator(OperatorRepresentation representation)
        {
            Operation operation = new(representation, _priority);
            _content.Operations.Add(operation);
            _content.UsedOperators |= representation.Priority;
        }
    }
}