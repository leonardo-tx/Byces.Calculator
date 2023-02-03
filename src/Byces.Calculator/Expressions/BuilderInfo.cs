using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly ref struct BuilderInfo
    {
        internal BuilderInfo(ReadOnlySpan<char> expressionSpan)
        {
            OperationCount = 0;
            UnclosedParentheses = 0;
            FunctionCount = 0;
            HasSpecialNumber = false;

            bool atNumber = false;
            bool isSpecialNumber = true;
            for (int i = 0, firstIndex = 0; i < expressionSpan.Length; i++, firstIndex++)
            {
                if (expressionSpan[i] == '(')
                {
                    UnclosedParentheses++;
                    continue;
                }
                if (expressionSpan[i] == ')')
                {
                    UnclosedParentheses--;
                    continue;
                }
                if (!atNumber && (expressionSpan[i] == '+' || expressionSpan[i] == '-'))
                {
                    atNumber = true;
                    continue;
                }
                if (char.IsDigit(expressionSpan[i]) || expressionSpan[i] == ',' || expressionSpan[i] == '.')
                {
                    atNumber = true;
                    isSpecialNumber = false;
                    continue;
                }
                if (isSpecialNumber && SpecialNumberType.TryParse(expressionSpan[firstIndex..(i + 1)], out _))
                {
                    firstIndex = i;
                    atNumber = true;
                    HasSpecialNumber = true;
                    isSpecialNumber = false;
                    continue;
                }
                if (atNumber && OperationType.TryParse(expressionSpan[firstIndex..(i + 1)], out _))
                {
                    OperationCount++;
                    atNumber = false;
                    isSpecialNumber = true;
                    firstIndex = i;
                    continue;
                }
                if (!atNumber && FunctionType.TryParse(expressionSpan[firstIndex..(i + 1)], out FunctionType functionType))
                {
                    if (functionType.AdditionalCheck > 0 && i + 1 != expressionSpan.Length && char.IsLetter(expressionSpan[i + 1]))
                    {
                        for (int j = 1; j <= functionType.AdditionalCheck; j++)
                        {
                            if (!FunctionType.TryParse(expressionSpan[firstIndex..(i + 1 + j)], out _)) continue;
                            i += j;
                        }
                    }
                    FunctionCount++;
                    firstIndex = i; continue;
                }
                firstIndex--;
            }
            HasFunction = FunctionCount > 0;
        }

        internal int UnclosedParentheses { get; }

        internal int OperationCount { get; }

        internal int FunctionCount { get; }

        internal bool HasSpecialNumber { get; }

        internal bool HasFunction { get; }
    }
}