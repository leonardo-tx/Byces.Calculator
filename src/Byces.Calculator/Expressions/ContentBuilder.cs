using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;

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
                    if (FindOperation(content, expressionSpan))
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

        private bool FindWhiteSpace(char currentChar)
        {
            if (!char.IsWhiteSpace(currentChar)) return false;
            if (FirstIndex != LastIndex) FirstIndex--;

            return true;
        }

        private bool FindParentheses(Content content, char currentChar)
        {
            if (currentChar == '(')
            {
                if (AfterNumber)
                {
                    _ = OperationType.TryParse('*', out OperationType operationType);
                    AddOperation(content, operationType);
                    
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
                if (char.IsDigit(currentChar) || currentChar == '.' || currentChar == ',') { continue; }
                if (char.IsWhiteSpace(currentChar)) { whiteSpaceCount++; continue; }
                if (!atScientificNotation && (currentChar == 'E' || currentChar == 'e')) { atScientificNotation = true; continue; }
                if (atScientificNotation && !hasSignal2 && (currentChar == '+' || currentChar == '-')) { hasSignal2 = true; continue; }
                break;
            }
            return ParseNumber(content, expressionSpan[FirstIndex..LastIndex], whiteSpaceCount);
        }

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

        private bool FindSpecialNumber(Content content, ReadOnlySpan<char> expressionSpan)
        {
            var parseResult = FindGeneric<VariableType>(expressionSpan);
            if (parseResult == null) return false;

            switch (parseResult.ResultType)
            {
                case ResultType.Number:
                    AddNumber(content, parseResult.GetNumber());
                    break;
                case ResultType.Boolean:
                    AddNumber(content, parseResult.GetBoolean());
                    break;
            }
            return true;
        }

        private bool FindFunction(Content content, ReadOnlySpan<char> expressionSpan)
        {
            var parseResult = FindGeneric<FunctionType>(expressionSpan);
            if (parseResult == null) return false;

            AddFunction(content, parseResult);
            return true;
        }

        private bool FindOperation(Content content, ReadOnlySpan<char> expressionSpan)
        {
            var parseResult = FindGeneric<OperationType>(expressionSpan);
            if (parseResult == null) return false;

            AddOperation(content, parseResult);
            return true;
        }

        private T? FindGeneric<T>(ReadOnlySpan<char> expressionSpan) where T : ExpressionType<T>
        {
            if (LastIndex == expressionSpan.Length) { return null; }

            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();

            if (currentSpan.Length - whiteSpaceCount > ExpressionType<T>.MaxStringSize) return null;
            if (whiteSpaceCount == 0)
            {
                return FindGenericNoWhiteSpace<T>(expressionSpan);
            }
            else
            {
                return FindGenericWithWhiteSpace<T>(expressionSpan, whiteSpaceCount);
            }
        }

        private T? FindGenericNoWhiteSpace<T>(ReadOnlySpan<char> expressionSpan) where T : ExpressionType<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            if (!ExpressionType<T>.TryParse(currentSpan, out var type)) return null;
            
            T? type2 = null; int increaseLastIndex = 0;
            for (int i = 1, whiteSpaceCount = 0; i <= type.AdditionalCheck && LastIndex + 1 + i + whiteSpaceCount != expressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(expressionSpan[LastIndex + i + whiteSpaceCount])) { i--; whiteSpaceCount++; continue; }
                currentSpan = expressionSpan[FirstIndex..(LastIndex + 1 + i + whiteSpaceCount)];

                var type3 = FindGenericWithWhitespaceAtLoopSpan<T>(currentSpan, whiteSpaceCount);
                if (type3 == null) continue;

                increaseLastIndex = i + whiteSpaceCount; type2 = type3;
            }
            if (type2 != null) { type = type2; LastIndex += increaseLastIndex; }
            return type;
        }

        private T? FindGenericWithWhiteSpace<T>(ReadOnlySpan<char> expressionSpan, int whiteSpaceCount) where T : ExpressionType<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
            for (int i = 0, j = 0; i < currentSpan.Length; i++)
            {
                if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
            }

            if (!ExpressionType<T>.TryParse(span, out var type)) return null;

            T? type2 = null; int increaseLastIndex = 0;
            for (int i = 1, whiteSpaceCount2 = 0; i <= type.AdditionalCheck && LastIndex + 1 + i + whiteSpaceCount2 != expressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(expressionSpan[LastIndex + i + whiteSpaceCount2])) { i--; whiteSpaceCount2++; continue; }
                currentSpan = expressionSpan[FirstIndex..(LastIndex + 1 + i + whiteSpaceCount2)];

                var type3 = FindGenericWithWhitespaceAtLoopSpan<T>(currentSpan, whiteSpaceCount + whiteSpaceCount2);
                if (type3 == null) continue;

                increaseLastIndex = i + whiteSpaceCount2; type2 = type3;
            }
            if (type2 != null) { type = type2; LastIndex += increaseLastIndex; }
            return type;
        }

        private T? FindGenericWithWhitespaceAtLoopSpan<T>(ReadOnlySpan<char> currentSpan, int whiteSpaceCountTotal) where T : ExpressionType<T>
        {
            Span<char> loopSpan = stackalloc char[currentSpan.Length - whiteSpaceCountTotal];
            for (int i = 0, j = 0; i < currentSpan.Length; i++)
            {
                if (!char.IsWhiteSpace(currentSpan[i])) loopSpan[j++] = currentSpan[i];
            }
            if (!ExpressionType<T>.TryParse(loopSpan, out var type)) return null;
            return type;
        }

        private void AddFunction(Content content, FunctionType functionType)
        {
            Function function = new Function(content.Values.Count, functionType, Priority);
            content.Functions.Add(function);
        }

        private void AddNumber(Content content, bool givenValue)
        {
            content.Values.Add(givenValue);
        }

        private void AddNumber(Content content, double givenValue)
        {
            content.Values.Add(IsNegative ? -givenValue : givenValue);
        }

        private void AddOperation(Content content, OperationType operationType)
        {
            Operation operation = new Operation(operationType, Priority);
            content.Operations.Add(operation);
        }
    }
}