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
        /// Returns the maximum priority of a <see cref="Function"/> span.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The maximum priority in the sequence, or -1 if source is empty.</returns>
        internal static int MaxPriority(this Span<Function?> source)
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