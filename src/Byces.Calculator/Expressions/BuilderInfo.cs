using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly ref struct BuilderInfo
    {
        private BuilderInfo(int unclosedParentheses, int operationCount, int functionCount, bool hasSpecialNumber, bool hasPriority)
        {
            UnclosedParentheses = unclosedParentheses;
            FunctionCount = functionCount;
            OperationCount = operationCount;
            HasSpecialNumber = hasSpecialNumber;
            HasFunction = functionCount > 0;
            HasPriority = hasPriority;
        }

        internal int UnclosedParentheses { get; }

        internal int OperationCount { get; }

        internal int FunctionCount { get; }

        internal bool HasSpecialNumber { get; }

        internal bool HasFunction { get; }

        internal bool HasPriority { get; }

        internal static BuilderInfo GetInfo(ReadOnlySpan<char> expressionSpan)
        {
            int operationCount = 0, unclosedParentheses = 0, functionCount = 0;
            bool hasSpecialNumber = false, afterNumber = false, hasPriority = false;

            for (int lastIndex = 0, firstIndex = 0; lastIndex < expressionSpan.Length; lastIndex++, firstIndex++)
            {
                if (FindParentheses(expressionSpan[lastIndex], ref unclosedParentheses, ref hasPriority)) continue;
                if (!afterNumber)
                {
                    if (FindNumber(expressionSpan, ref firstIndex, ref lastIndex)) { afterNumber = true; continue; }
                    if (FindSpecialNumber(expressionSpan, ref firstIndex, ref lastIndex)) { afterNumber = true; hasSpecialNumber = true; continue; }
                    if (FindFunction(expressionSpan, ref firstIndex, ref lastIndex)) { firstIndex = lastIndex; functionCount++; continue; }
                }
                else
                {
                    if (FindOperation(expressionSpan, firstIndex, lastIndex)) { firstIndex = lastIndex; afterNumber = false; operationCount++; continue; }
                }
                firstIndex--;
            }
            return new BuilderInfo(unclosedParentheses, operationCount, functionCount, hasSpecialNumber, hasPriority);
        }

        private static bool FindParentheses(char currentChar, ref int unclosedParentheses, ref bool hasPriority)
        {
            if (currentChar == '(')
            {
                unclosedParentheses++;
                hasPriority = true;
                return true;
            }
            if (currentChar == ')')
            {
                unclosedParentheses--;
                hasPriority = true;
                return true;
            }
            return false;
        }

        private static bool FindNumber(ReadOnlySpan<char> expressionSpan, ref int firstIndex, ref int lastIndex)
        {
            if (lastIndex != firstIndex) return false;
            bool hasSignal = expressionSpan[lastIndex] == '+' || expressionSpan[lastIndex] == '-';

            if (hasSignal) lastIndex++;
            while (lastIndex < expressionSpan.Length && (char.IsDigit(expressionSpan[lastIndex]) || expressionSpan[lastIndex] == '.' || expressionSpan[lastIndex] == ','))
            {
                lastIndex++;
                if (lastIndex == expressionSpan.Length) break;
                if (expressionSpan[lastIndex] == 'E' || expressionSpan[lastIndex] == 'e')
                {
                    if (lastIndex + 1 == expressionSpan.Length) throw new UnknownNumberExpressionException();
                    if (expressionSpan[lastIndex + 1] != '+' && expressionSpan[lastIndex + 1] != '-') throw new UnknownNumberExpressionException();

                    lastIndex += 2;
                }
            }
            if (lastIndex == firstIndex) return false;
            if (lastIndex - firstIndex == 1 && hasSignal) return false;

            firstIndex = --lastIndex;
            return true;
        }

        private static bool FindSpecialNumber(ReadOnlySpan<char> expressionSpan, ref int firstIndex, ref int lastIndex)
        {
            if (lastIndex == expressionSpan.Length || !SpecialNumberType.TryParse(expressionSpan[firstIndex..(lastIndex + 1)], out _)) return false;
            
            firstIndex = lastIndex;
            return true;
        }

        private static bool FindFunction(ReadOnlySpan<char> expressionSpan, ref int firstIndex, ref int lastIndex)
        {
            if (lastIndex == expressionSpan.Length || !FunctionType.TryParse(expressionSpan[firstIndex..(lastIndex + 1)], out FunctionType functionType)) return false;
            if (functionType.AdditionalCheck > 0 && lastIndex + 1 != expressionSpan.Length && char.IsLetter(expressionSpan[lastIndex + 1]))
            {
                for (int i = 1; i <= functionType.AdditionalCheck; i++)
                {
                    if (!FunctionType.TryParse(expressionSpan[firstIndex..(lastIndex + 1 + i)], out _)) continue;

                    lastIndex += i;
                    return true;
                }
            }
            return true;
        }

        private static bool FindOperation(ReadOnlySpan<char> expressionSpan, int firstIndex, int lastIndex)
        {
            if (OperationType.TryParse(expressionSpan[firstIndex..(lastIndex + 1)], out _)) return true;
            return false;
        }
    }
}