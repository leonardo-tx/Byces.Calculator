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

        internal static int Count(this ReadOnlySpan<char> source, char character)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == character) count++;
            }
            return count;
        }

        internal static int CountWhiteSpaces(this ReadOnlySpan<char> source)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (char.IsWhiteSpace(source[i])) count++;
            }
            return count;
        }

        internal static (int, int) CountOperationsAndSelfOperations(this ReadOnlySpan<char> source)
        {
            int operationsCount = 0;
            int selfOperationsCount = 0;

            bool atNumber = false;
            bool isSpecialNumber = true;
            for (int i = 0, firstIndex = 0; i < source.Length; i++, firstIndex++)
            {
                if (source[i] == '(' || source[i] == ')') { firstIndex = i; continue; }
                if (!atNumber && (source[i] == '+' || source[i] == '-')) { atNumber = true; continue; }
                if (char.IsDigit(source[i])) { firstIndex = i; atNumber = true; isSpecialNumber = false; continue; }
                if (isSpecialNumber && Number.TryParse(source[firstIndex..(i + 1)], out _)) { firstIndex = i; atNumber = true; continue; }
                if (atNumber && OperationType.TryParse(source[firstIndex..(i + 1)], out _)) { operationsCount++; atNumber = false; isSpecialNumber = true; firstIndex = i; continue; }
                if (!atNumber && SelfOperationType.TryParse(source[firstIndex..(i + 1)], out SelfOperationType selfOperationType))
                {
                    if (selfOperationType.AdditionalCheck > 0 && i + 1 != source.Length && char.IsLetter(source[i + 1]))
                    {
                        for (int j = 1; j <= selfOperationType.AdditionalCheck; j++)
                        {
                            if (!SelfOperationType.TryParse(source[firstIndex..(i + 1 + j)], out _)) continue;

                            i += j;
                        }
                    }
                    selfOperationsCount++;
                    firstIndex = i; continue;
                }
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

        /// <summary>
        /// Returns the maximum priority of an <see cref="SelfOperation"/> span.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The maximum priority in the sequence, or -1 if source is empty.</returns>
        internal static int MaxPriority(this Span<SelfOperation?> source)
        {
            int max = -1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == null) continue;
                if (source[i]!.Value.Priority > max) max = source[i]!.Value.Priority;
            }
            return max;
        }
    }
}