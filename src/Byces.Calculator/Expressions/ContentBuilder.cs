using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using Byces.Calculator.Representations;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Byces.Calculator.Expressions
{
    internal ref struct ContentBuilder
    {
        private const int StackAllocationLimit = 512;

        private int FirstIndex;

        private int LastIndex;

        private int Priority;

        private bool AfterNumber;

        private bool IsNegative;

        private char GroupSeparator;

        private char DecimalSeparator;

        internal void Build(Content content, ReadOnlySpan<char> expressionSpan)
        {
            GroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator[0];
            DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            for (; LastIndex < expressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindWhiteSpace(expressionSpan[LastIndex])) continue;
                if (FindParentheses(content, expressionSpan[LastIndex])) continue;
                if (!AfterNumber)
                {
                    if (FindNumber(content, expressionSpan) || FindVariable(content, expressionSpan))
                    {
                        AfterNumber = true; FirstIndex = LastIndex;
                        continue;
                    }
                    if (FindFunction(content, expressionSpan)) { FirstIndex = LastIndex; continue; }
                }
                else
                {
                    if (FindOperator(content, expressionSpan))
                    {
                        FirstIndex = LastIndex; AfterNumber = false;
                        continue;
                    }
                }
                FirstIndex--;
            }
            if (Priority > 0) throw new MissingParenthesesExpressionException();
            if (!AfterNumber) throw new IncompleteExpressionException();
            if (LastIndex != FirstIndex) throw new UnknownNumberExpressionException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindWhiteSpace(char currentChar)
        {
            if (!char.IsWhiteSpace(currentChar)) return false;
            if (FirstIndex != LastIndex) FirstIndex--;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindParentheses(Content content, char currentChar)
        {
            if (currentChar == '(')
            {
                if (AfterNumber)
                {
                    AddOperator(content, OperatorRepresentation.Parse("*"));
                    AfterNumber = false;
                }
                Priority++;
                return true;
            }
            if (currentChar == ')')
            {
                if (--Priority < 0) throw new MisplacedParenthesesExpressionException();
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindNumber(Content content, ReadOnlySpan<char> expressionSpan)
        {
            if (FirstIndex != LastIndex) return false;
            
            IsNegative = expressionSpan[LastIndex] == '-';
            bool hasSignal = IsNegative || expressionSpan[LastIndex] == '+';

            if (hasSignal) { LastIndex++; FirstIndex++; }
            if (char.IsLetter(expressionSpan[LastIndex])) return false;
            int whiteSpaceCount = 0; NumberStyles numberStyles = NumberStyles.None;
            for (bool atScientificNotation = false, atDecimal = false, hasSignal2 = false; LastIndex < expressionSpan.Length; LastIndex++)
            {
                char currentChar = expressionSpan[LastIndex];
                if (char.IsDigit(currentChar)) continue;
                if (!atDecimal && currentChar == GroupSeparator) { numberStyles |= NumberStyles.AllowThousands; continue; }
                if (!atDecimal && currentChar == DecimalSeparator) { atDecimal = true; numberStyles |= NumberStyles.AllowDecimalPoint; continue; }
                if (char.IsWhiteSpace(currentChar)) { whiteSpaceCount++; continue; }
                if (!atScientificNotation && (currentChar == 'E' || currentChar == 'e')) { atScientificNotation = true; numberStyles |= NumberStyles.AllowExponent; continue; }
                if (atScientificNotation && !hasSignal2 && (currentChar == '+' || currentChar == '-')) { hasSignal2 = true; continue; }
                break;
            }
            return ParseNumber(content, expressionSpan[FirstIndex..LastIndex], whiteSpaceCount, numberStyles);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ParseNumber(Content content, ReadOnlySpan<char> currentSpan, int whiteSpaceCount, NumberStyles numberStyles)
        {
            if (FirstIndex == LastIndex) return false;
            LastIndex--;

            bool parseResult; double doubleResult = double.NaN; long longResult = 0;
            if (whiteSpaceCount == 0)
            {
                parseResult = double.TryParse(currentSpan, numberStyles, null, out doubleResult);
            }
            else
            {
                int count = currentSpan.Length - whiteSpaceCount;
                Span<char> span = (StackAllocationLimit >= count) ? stackalloc char[count] : new char[count];
                for (int i = 0, j = 0; i < currentSpan.Length; i++)
                {
                    if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
                }
                parseResult = double.TryParse(span, numberStyles, null, out doubleResult);
            }
            if (!parseResult) throw new UnknownNumberExpressionException();
            if (double.IsNaN(doubleResult))
            {
                AddNumber(content, longResult);
                return true;
            }
            AddNumber(content, doubleResult);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindVariable(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<VariableRepresentation>(expressionSpan, ExpressionConflict.Variable, out bool isType);
            if (!isType || value == -1) return false;

            AddNumber(content, VariableRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindFunction(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<FunctionRepresentation>(expressionSpan, ExpressionConflict.Function, out bool isType);
            if (!isType)
            {
                if (value == -1) return false;
                else { LastIndex--; return false; }
            }
            AddFunction(content, FunctionRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindOperator(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<OperatorRepresentation>(expressionSpan, ExpressionConflict.Operator, out _);
            if (value == -1) return false;

            AddOperator(content, OperatorRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValue<T>(ReadOnlySpan<char> expressionSpan, ExpressionConflict resultConflict, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            if (LastIndex == expressionSpan.Length) { isOriginalType = false; return -1; }

            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();

            if (currentSpan.Length - whiteSpaceCount > ExpressionRepresentation<T>.MaxStringSize) { isOriginalType = false; return -1; }
            if (whiteSpaceCount == 0)
            {
                return FindGenericValueNoWhiteSpace<T>(expressionSpan, resultConflict, out isOriginalType);
            }
            return FindGenericValueWithWhiteSpace<T>(expressionSpan, whiteSpaceCount, resultConflict, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValueNoWhiteSpace<T>(ReadOnlySpan<char> expressionSpan, ExpressionConflict resultConflict, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            if (!ExpressionRepresentation<T>.TryParse(currentSpan, out var representation)) { isOriginalType = false; return -1; }

            return FindConflicts(expressionSpan, representation, resultConflict, currentSpan.Length, 0, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValueWithWhiteSpace<T>(ReadOnlySpan<char> expressionSpan, int whiteSpaceCount, ExpressionConflict resultConflict, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
            for (int i = 0, j = 0; i < currentSpan.Length; i++)
            {
                if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
            }
            if (!ExpressionRepresentation<T>.TryParse(span, out var representation)) { isOriginalType = false; return -1; }

            return FindConflicts(expressionSpan, representation, resultConflict, span.Length, whiteSpaceCount, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindConflicts<T>(ReadOnlySpan<char> expressionSpan, T firstResult, ExpressionConflict resultConflict, int foundLength, int additionalWhiteSpaceCount, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            int value = -1, increaseLastIndex = 0; isOriginalType = true;
            for (int i = 0; i < firstResult.representableConflicts.Length; i++)
            {
                var representableConflict = firstResult.representableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1, whiteSpaceCount = 0;
                for (; j <= representableConflict.Difference && LastIndex + j + whiteSpaceCount < expressionSpan.Length; j++)
                {
                    if (char.IsWhiteSpace(expressionSpan[LastIndex + j + whiteSpaceCount])) { j--; whiteSpaceCount++; continue; }
                }
                ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + --j + whiteSpaceCount + 1)];
                
                int value2 = GetGenericValue(currentSpan, additionalWhiteSpaceCount + whiteSpaceCount, representableConflict.ExpressionConflict);
                if (value2 == -1 || increaseLastIndex > j + whiteSpaceCount) continue;

                isOriginalType = representableConflict.ExpressionConflict == resultConflict;
                increaseLastIndex = j + whiteSpaceCount;
                value = value2;
            }
            if (value != -1)
            {
                LastIndex += increaseLastIndex;
                return value;
            }
            return firstResult.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetGenericValue(ReadOnlySpan<char> currentSpan, int whiteSpaceCountTotal, ExpressionConflict conflict)
        {
            if (whiteSpaceCountTotal == 0) return ParseToValue(currentSpan, conflict);

            Span<char> loopSpan = stackalloc char[currentSpan.Length - whiteSpaceCountTotal];
            for (int i = 0, j = 0; i < currentSpan.Length; i++)
            {
                if (!char.IsWhiteSpace(currentSpan[i])) loopSpan[j++] = currentSpan[i];
            }
            return ParseToValue(loopSpan, conflict);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseToValue(ReadOnlySpan<char> span, ExpressionConflict conflict)
        {
            switch (conflict)
            {
                case ExpressionConflict.Operator:
                    if (OperatorRepresentation.TryParse(span, out var operatorRepresentation)) return operatorRepresentation.Value;
                    break;
                case ExpressionConflict.Function:
                    if (FunctionRepresentation.TryParse(span, out var functionRepresentation)) return functionRepresentation.Value;
                    break;
                case ExpressionConflict.Variable:
                    if (VariableRepresentation.TryParse(span, out var variableRepresentation)) return variableRepresentation.Value;
                    break;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddFunction(Content content, FunctionRepresentation representation)
        {
            Function function = new Function(content.Variables.Count, representation, Priority);
            content.Functions.Add(function);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(Content content, VariableRepresentation representation)
        {
            var value = representation.GetValue();
            if (IsNegative && value.Type == Enums.VariableType.Number)
            {
                content.Variables.Add(-value.Double);
                return;
            }
            content.Variables.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(Content content, double givenValue)
        {
            content.Variables.Add(IsNegative ? -givenValue : givenValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddOperator(Content content, OperatorRepresentation representation)
        {
            Operation operation = new Operation(representation, Priority);
            content.Operations.Add(operation);
            content.UsedOperators |= representation.Priority;
        }
    }
}