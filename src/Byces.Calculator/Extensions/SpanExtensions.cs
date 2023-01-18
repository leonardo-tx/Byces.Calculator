using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Extensions
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

        internal static (int, int) CountOperationsAndSelfOperations(this ReadOnlySpan<char> source)
        {
            int operationsCount = 0;
            int selfOperationsCount = 0;

            bool isNumber = false;
            for (int i = 0, firstIndex = 0; i < source.Length; i++, firstIndex++)
            {
                if (source[i] == '(' || source[i] == ')') { firstIndex = i; continue; }
                if (!isNumber && (source[i] == '+' || source[i] == '-')) { isNumber = true; continue; }
                if (char.IsDigit(source[i]) || Number.TryParse(source[firstIndex..(i + 1)], out _)) { firstIndex = i; isNumber = true; continue; }
                if (isNumber && OperationType.TryParse(source[firstIndex..(i + 1)], OperationCategory.None, out _)) { operationsCount++; isNumber = false; firstIndex = i; continue; }
                if (!isNumber && OperationType.TryParse(source[firstIndex..(i + 1)], OperationCategory.Before, out _)) { selfOperationsCount++; firstIndex = i; continue; }
                firstIndex--;
            }
            return (operationsCount, selfOperationsCount);
        }

        internal static bool CustomContains(this ReadOnlySpan<int> source, int other)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == other) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the maximum priority of an <see cref="Operation"/> span.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The maximum priority in the sequence, or -1 if source is empty.</returns>
        internal static int MaxPriority(this Span<Operation?> source)
        {
            int max = -1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == null) continue;
                if (source[i]!.Value.Priority > max) max = source[i]!.Value.Priority;
            }
            return max;
        }

        internal static int MaxPriority(this Span<SelfOperation?> source)
        {
            int max = -1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == null) continue;
                if (source[i]!.Value.Operation.Priority > max) max = source[i]!.Value.Operation.Priority;
            }
            return max;
        }
    }
}