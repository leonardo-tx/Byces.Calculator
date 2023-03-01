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

        private double CurrentNumber;

        internal void Build(Content content, ReadOnlySpan<char> expressionSpan)
        {
            CurrentNumber = double.NaN;
            for (; LastIndex < expressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindWhiteSpace(expressionSpan[LastIndex])) continue;
                if (FindParentheses(content, expressionSpan[LastIndex])) continue;
                if (!AfterNumber)
                {
                    if (FindNumber(expressionSpan) || FindSpecialNumber(expressionSpan))
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
                        FirstIndex = LastIndex; AfterNumber = false; CurrentNumber = double.NaN;
                        continue;
                    }
                }
                FirstIndex--;
            }
            if (Priority > 0) throw new MissingParenthesesExpressionException();
            AddNumber(content);
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
                    AddNumber(content);
                    AddOperation(content, OperationType.Multiply);
                    
                    AfterNumber = false; CurrentNumber = double.NaN;
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

        private bool FindNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (FirstIndex != LastIndex) return false;
            bool hasSignal = expressionSpan[LastIndex] == '+' || expressionSpan[LastIndex] == '-';

            if (hasSignal) LastIndex++;
            if (expressionSpan[LastIndex] == 'e' || expressionSpan[LastIndex] == 'E') return false;
            int whiteSpaceCount = 0;
            for (bool foundNumber = false; LastIndex < expressionSpan.Length; LastIndex++)
            {
                char currentChar = expressionSpan[LastIndex];
                if (char.IsDigit(currentChar) || currentChar == '.' || currentChar == ',') { foundNumber = true; continue; }
                if (currentChar == 'E' || currentChar == 'e')
                {
                    if (LastIndex + 2 >= expressionSpan.Length) throw new UnknownNumberExpressionException();
                    char nextChar = expressionSpan[LastIndex + 1];
                    if (nextChar != '+' && nextChar != '-') throw new UnknownNumberExpressionException();
                    if (!char.IsDigit(expressionSpan[LastIndex + 2])) throw new UnknownNumberExpressionException();

                    LastIndex += 3; continue;
                }
                if (foundNumber && char.IsWhiteSpace(currentChar)) { whiteSpaceCount++; continue; }
                break;
            }
            return ParseNumber(expressionSpan[FirstIndex..LastIndex], hasSignal, whiteSpaceCount);
        }

        private bool ParseNumber(ReadOnlySpan<char> currentSpan, bool hasSignal, int whiteSpaceCount)
        {
            if (FirstIndex == LastIndex || (LastIndex - FirstIndex == 1 && hasSignal)) return false;
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
            CurrentNumber = result; return true;
        }

        private bool FindSpecialNumber(ReadOnlySpan<char> expressionSpan)
        {
            if (LastIndex == expressionSpan.Length) return false;

            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();
            
            if (currentSpan.Length - whiteSpaceCount > SpecialNumberType.MaxStringSize + 1) return false;
            
            bool parseResult; double result;
            if (whiteSpaceCount == 0) parseResult = SpecialNumberType.TryParse(currentSpan, out result);
            else
            {
                Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
                for (int i = 0, j = 0; i < currentSpan.Length; i++)
                {
                    if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
                }
                parseResult = SpecialNumberType.TryParse(span, out result);
            }
            CurrentNumber = result; return parseResult;
        }

        private bool FindFunction(Content content, ReadOnlySpan<char> expressionSpan)
        {
            if (LastIndex == expressionSpan.Length) return false;

            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();

            if (currentSpan.Length - whiteSpaceCount > FunctionType.MaxStringSize + 1) return false;
            if (whiteSpaceCount == 0)
            {
                if (!FunctionType.TryParse(currentSpan, out var functionType)) return false;
                if (functionType.AdditionalCheck > 0 && LastIndex + 1 != expressionSpan.Length && char.IsLetter(expressionSpan[LastIndex + 1]))
                {
                    for (int i = 1; i <= functionType.AdditionalCheck; i++)
                    {
                        if (!FunctionType.TryParse(expressionSpan[FirstIndex..(LastIndex + 1 + i)], out FunctionType functionType2)) continue;

                        LastIndex += i;
                        AddFunction(content, functionType2);
                        return true;
                    }
                }
                AddFunction(content, functionType);
                return true;
            }
            else
            {
                Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
                for (int i = 0, j = 0; i < currentSpan.Length; i++)
                {
                    if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
                }

                if (!FunctionType.TryParse(span, out var functionType)) return false;
                while (char.IsWhiteSpace(expressionSpan[++LastIndex])) { whiteSpaceCount++; continue; }
                LastIndex--;
                if (functionType.AdditionalCheck > 0 && LastIndex + 1 != expressionSpan.Length && char.IsLetter(expressionSpan[LastIndex + 1]))
                {
                    for (int i = 1, whiteSpaceCount2 = 0; i <= functionType.AdditionalCheck; i++)
                    {
                        if (char.IsWhiteSpace(expressionSpan[LastIndex + i])) { i--; whiteSpaceCount2++; continue; }
                        currentSpan = expressionSpan[FirstIndex..(LastIndex + 1 + i + whiteSpaceCount2)];
                        span = stackalloc char[currentSpan.Length - whiteSpaceCount2 - whiteSpaceCount];
                        for (int j = 0, k = 0; j < currentSpan.Length; j++)
                        {
                            if (!char.IsWhiteSpace(currentSpan[j])) span[k++] = currentSpan[j];
                        }
                        if (!FunctionType.TryParse(span, out var functionType2)) continue;
                        LastIndex += i;
                        AddFunction(content, functionType2);
                        return true;
                    }
                }
                AddFunction(content, functionType);
                return true;
            }
        }

        private bool FindOperation(Content content, ReadOnlySpan<char> expressionSpan)
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[FirstIndex..(LastIndex + 1)];
            int whiteSpaceCount = currentSpan.CountWhiteSpaces();

            if (currentSpan.Length - whiteSpaceCount > OperationType.MaxStringSize) return false;
            bool parseResult; OperationType result;
            if (whiteSpaceCount == 0) parseResult = OperationType.TryParse(currentSpan, out result);
            else
            {
                Span<char> span = stackalloc char[currentSpan.Length - whiteSpaceCount];
                for (int i = 0, j = 0; i < currentSpan.Length; i++)
                {
                    if (!char.IsWhiteSpace(currentSpan[i])) span[j++] = currentSpan[i];
                }
                parseResult = OperationType.TryParse(span, out result);
            }
            if (parseResult) { AddNumber(content); AddOperation(content, result); }
            return parseResult;
        }

        private void AddFunction(Content content, FunctionType functionType)
        {
            Function function = new Function(content.Numbers.Count, functionType, Priority);
            content.Functions.Add(function);
        }

        private void AddNumber(Content content)
        {
            if (double.IsNaN(CurrentNumber)) throw new IncompleteExpressionException();
            content.Numbers.Add(CurrentNumber);
        }

        private void AddOperation(Content content, OperationType operationType)
        {
            Operation operation = new Operation(operationType, Priority);
            content.Operations.Add(operation);
        }
    }
}