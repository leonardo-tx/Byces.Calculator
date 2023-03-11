using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;
using System.Runtime.CompilerServices;

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

        internal void Build(Content content, ReadOnlySpan<char> expressionSpan)
        {
            for (; LastIndex < expressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindWhiteSpace(expressionSpan[LastIndex])) continue;
                if (FindParentheses(content, expressionSpan[LastIndex])) continue;
                if (!AfterNumber)
                {
                    if (FindNumber(content, expressionSpan) || FindSpecialNumber(content, expressionSpan))
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
                    OperatorRepresentation.TryParse("*", out OperatorRepresentation operationType);
                    AddOperator(content, operationType);
                    
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
            if (expressionSpan[LastIndex] == 'e' || expressionSpan[LastIndex] == 'E') return false;
            int whiteSpaceCount = 0;
            for (bool atScientificNotation = false, hasSignal2 = false; LastIndex < expressionSpan.Length; LastIndex++)
            {
                char currentChar = expressionSpan[LastIndex];
                if (char.IsDigit(currentChar) || currentChar == '.' || currentChar == ',') continue;
                if (char.IsWhiteSpace(currentChar)) { whiteSpaceCount++; continue; }
                if (!atScientificNotation && (currentChar == 'E' || currentChar == 'e')) { atScientificNotation = true; continue; }
                if (atScientificNotation && !hasSignal2 && (currentChar == '+' || currentChar == '-')) { hasSignal2 = true; continue; }
                break;
            }
            return ParseNumber(content, expressionSpan[FirstIndex..LastIndex], whiteSpaceCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ParseNumber(Content content, ReadOnlySpan<char> currentSpan, int whiteSpaceCount)
        {
            if (FirstIndex == LastIndex) return false;
            LastIndex--;

            bool parseResult; double result;
            if (whiteSpaceCount == 0) parseResult = double.TryParse(currentSpan, out result);
            else
            {
                int count = currentSpan.Length - whiteSpaceCount;
                Span<char> span = (StackAllocationLimit >= count) ? stackalloc char[count] : new char[count];
                for (int i = 0, j = 0; i < currentSpan.Length; i++)
                {
                    if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
                }
                parseResult = double.TryParse(span, out result);
            }
            if (!parseResult) throw new UnknownNumberExpressionException();

            AddNumber(content, result);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindSpecialNumber(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<VariableRepresentation>(expressionSpan, out bool isType);
            if (!isType || value == -1) return false;

            AddNumber(content, VariableRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindFunction(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int value = FindGenericValue<FunctionRepresentation>(expressionSpan, out bool isType);
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
            int value = FindGenericValue<OperatorRepresentation>(expressionSpan, out _);
            if (value == -1) return false;

            AddOperator(content, OperatorRepresentation.GetItem(value));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValue<T>(ReadOnlySpan<char> expressionSpan, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            if (LastIndex == expressionSpan.Length) { isOriginalType = false; return -1; }

            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();

            if (currentSpan.Length - whiteSpaceCount > ExpressionRepresentation<T>.MaxStringSize) { isOriginalType = false; return -1; }
            if (whiteSpaceCount == 0)
            {
                return FindGenericValueNoWhiteSpace<T>(expressionSpan, out isOriginalType);
            }
            return FindGenericValueWithWhiteSpace<T>(expressionSpan, whiteSpaceCount, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValueNoWhiteSpace<T>(ReadOnlySpan<char> expressionSpan, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            if (!ExpressionRepresentation<T>.TryParse(currentSpan, out var representation)) { isOriginalType = false; return -1; }

            return FindConflicts(expressionSpan, representation, currentSpan.Length, 0, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindGenericValueWithWhiteSpace<T>(ReadOnlySpan<char> expressionSpan, int whiteSpaceCount, out bool isOriginalType) where T : ExpressionRepresentation<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
            for (int i = 0, j = 0; i < currentSpan.Length; i++)
            {
                if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
            }
            if (!ExpressionRepresentation<T>.TryParse(span, out var representation)) { isOriginalType = false; return -1; }

            return FindConflicts(expressionSpan, representation, span.Length, whiteSpaceCount, out isOriginalType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindConflicts<T>(ReadOnlySpan<char> expressionSpan, T firstResult, int foundLength, int additionalWhiteSpaceCount, out bool isOriginalType) where T : ExpressionRepresentation<T>
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

                isOriginalType = representableConflict.ExpressionConflict == firstResult.RepresentationConflict;
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
            Function function = new Function(content.Values.Count, representation, Priority);
            content.Functions.Add(function);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(Content content, VariableRepresentation representation)
        {
            var value = representation.GetValue();
            if (IsNegative && value.ResultType == ResultType.Number)
            {
                content.Values.Add(-value.number);
                return;
            }
            content.Values.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddNumber(Content content, double givenValue)
        {
            content.Values.Add(IsNegative ? -givenValue : givenValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddOperator(Content content, OperatorRepresentation representation)
        {
            Operation operation = new Operation(representation, Priority);
            content.Operations.Add(operation);
        }
    }
}