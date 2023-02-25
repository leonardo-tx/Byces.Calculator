using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal static class ContentBuilder
    {
        internal static void Build(Content content, ReadOnlySpan<char> expressionSpan)
        {
            int priority = 0; double currentNumber = double.NaN; bool afterNumber = false;
            for (int firstIndex = 0, lastIndex = 0; lastIndex < expressionSpan.Length; lastIndex++, firstIndex++)
            {
                if (FindParentheses(expressionSpan[lastIndex], ref priority)) continue;
                if (!afterNumber)
                {
                    if (FindNumber(expressionSpan, firstIndex, ref lastIndex, out currentNumber) || 
                        FindSpecialNumber(expressionSpan, firstIndex, lastIndex, out currentNumber))
                    {
                        afterNumber = true; firstIndex = lastIndex;
                        continue;
                    }
                    if (FindFunction(content, expressionSpan, priority, firstIndex, ref lastIndex)) { firstIndex = lastIndex; continue; }
                }
                else
                {
                    if (FindOperation(content, expressionSpan[firstIndex..(lastIndex + 1)], currentNumber, priority))
                    {
                        firstIndex = lastIndex; afterNumber = false; currentNumber = double.NaN;
                        continue;
                    }
                }
                firstIndex--;
            }
            if (priority > 0) throw new MissingParenthesesExpressionException();
            AddNumber(content, currentNumber);
        }

        private static bool FindParentheses(char currentChar, ref int priority)
        {
            if (currentChar == '(')
            {
                priority++;
                return true;
            }
            if (currentChar == ')')
            {
                if (--priority < 0) throw new MisplacedParenthesesExpressionException();
                return true;
            }
            return false;
        }

        private static bool FindNumber(ReadOnlySpan<char> expressionSpan, int firstIndex, ref int lastIndex, out double currentNumber)
        {
            if (lastIndex != firstIndex) { currentNumber = double.NaN; return false; }
            bool hasSignal = expressionSpan[lastIndex] == '+' || expressionSpan[lastIndex] == '-';

            if (hasSignal) lastIndex++;
            if (expressionSpan[lastIndex] == 'e' || expressionSpan[lastIndex] == 'E') { currentNumber = double.NaN; return false; }
            for (; lastIndex < expressionSpan.Length; lastIndex++)
            {
                char currentChar = expressionSpan[lastIndex];

                if (char.IsDigit(currentChar) || currentChar == '.' || currentChar == ',') continue;
                if (currentChar == 'E' || currentChar == 'e')
                {
                    if (lastIndex + 2 >= expressionSpan.Length) throw new UnknownNumberExpressionException();
                    if (expressionSpan[lastIndex + 1] != '+' && expressionSpan[lastIndex + 1] != '-') throw new UnknownNumberExpressionException();
                    if (!char.IsDigit(expressionSpan[lastIndex + 2])) throw new UnknownNumberExpressionException();

                    lastIndex += 3; continue;
                }
                break;
            }
            if (lastIndex == firstIndex) { currentNumber = double.NaN; return false; }
            if (lastIndex - firstIndex == 1 && hasSignal) { currentNumber = double.NaN; return false; }

            currentNumber = double.Parse(expressionSpan[firstIndex..(--lastIndex + 1)]);
            return true;
        }

        private static bool FindSpecialNumber(ReadOnlySpan<char> expressionSpan, int firstIndex, int lastIndex, out double currentNumber)
        {
            if (lastIndex == expressionSpan.Length || !SpecialNumberType.TryParse(expressionSpan[firstIndex..(lastIndex + 1)], out currentNumber))
            {
                currentNumber = double.NaN;
                return false;
            }
            return true;
        }

        private static bool FindFunction(Content content, ReadOnlySpan<char> expressionSpan, int priority, int firstIndex, ref int lastIndex)
        {
            if (lastIndex == expressionSpan.Length || !FunctionType.TryParse(expressionSpan[firstIndex..(lastIndex + 1)], out FunctionType functionType)) return false;
            if (functionType.AdditionalCheck > 0 && lastIndex + 1 != expressionSpan.Length && char.IsLetter(expressionSpan[lastIndex + 1]))
            {
                for (int i = 1; i <= functionType.AdditionalCheck; i++)
                {
                    if (!FunctionType.TryParse(expressionSpan[firstIndex..(lastIndex + 1 + i)], out FunctionType functionType2)) continue;

                    lastIndex += i;
                    AddFunction(content, functionType2, priority);
                    return true;
                }
            }
            AddFunction(content, functionType, priority);
            return true;
        }

        private static void AddFunction(Content content, FunctionType functionType, int priority)
        {
            Function function = new Function(content.Numbers.Count, functionType, priority);
            content.Functions.Add(function);
        }

        private static bool FindOperation(Content content, ReadOnlySpan<char> currentSpan, double currentNumber, int priority)
        {
            if (OperationType.TryParse(currentSpan, out var operationType))
            {
                AddNumber(content, currentNumber);
                AddOperation(content, operationType, priority);
                return true;
            }
            return false;
        }

        private static void AddNumber(Content content, double currentNumber)
        {
            if (double.IsNaN(currentNumber)) throw new IncompleteExpressionException();
            content.Numbers.Add(currentNumber);
        }

        private static void AddOperation(Content content, OperationType operationType, int priority)
        {
            Operation operation = new Operation(operationType, priority);
            content.Operations.Add(operation);
        }
    }
}