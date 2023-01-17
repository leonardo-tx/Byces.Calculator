using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Extensions.ExpressionBuilder
{
    internal static class SpanExtensions
    {
        internal static int Count(this ReadOnlySpan<char> source, ReadOnlySpan<char> characters)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (source[i] == characters[j]) count++;
                }
            }
            return count;
        }

        internal static int CountOperations(this ReadOnlySpan<char> source)
        {
            int count = 0;
            for (int i = 0, firstIndex = 0; i < source.Length; i++)
            {
                if (char.IsDigit(source[i])) { firstIndex = i + 1; continue; }
                if (char.IsLetter(source[i]))
                {
                    if (OperationType.TryParse(source[firstIndex..(i + 1)], OperationCategory.None, out _)) { count++; firstIndex = i + 1; }
                    continue;
                }
                if (OperationType.TryParse(source.Slice(i, 1), OperationCategory.None, out _)) count++;
                firstIndex++;
            }
            return count;
        }

        internal static bool CustomContains(this ReadOnlySpan<int> source, int other)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == other) return true;
            }
            return false;
        }
    }
}