using Byces.Calculator.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Extensions
{
    internal static class CollectionExtensions
    {
        internal static bool ContainsAny(this ReadOnlySpan<char> source, ReadOnlySpan<char> chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                for (int j = 0; j < source.Length; j++)
                {
                    if (c == source[j]) return true;
                }
            }
            return false;
        }

        internal static bool ContainsWhiteSpace(this ReadOnlySpan<char> source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (char.IsWhiteSpace(source[i])) return true;
            }
            return false;
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

        internal static int MaxPriority(this List<Operation> source)
        {
            int max = -1;
#if NET5_0_OR_GREATER
            ReadOnlySpan<Operation> span = CollectionsMarshal.AsSpan(source);
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i].Priority > max) max = span[i].Priority;
            }
#else
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].Priority > max) max = source[i].Priority;
            }
#endif
            return max;
        }

        internal static int MaxPriority(this Span<Operation> source)
        {
            int max = -1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Priority > max) max = source[i].Priority;
            }
            return max;
        }

        internal static int MaxPriority(this List<Function> source)
        {
            int max = -1;
#if NET5_0_OR_GREATER
            ReadOnlySpan<Function> span = CollectionsMarshal.AsSpan(source);
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i].Priority > max) max = span[i].Priority;
            }
#else
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].Priority > max) max = source[i].Priority;
            }
#endif
            return max;
        }
    }
}