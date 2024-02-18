using Byces.Calculator.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Extensions
{
    internal static class CollectionExtensions
    {
        internal static bool Any(this ReadOnlySpan<char> source, Func<char, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i])) return true;
            }
            return false;
        }

        internal static int MaxPriority(this List<Function> source)
        {
            int max = -1;
            ReadOnlySpan<Function> span = CollectionsMarshal.AsSpan(source);
            
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i].Priority > max) max = span[i].Priority;
            }
            return max;
        }
    }
}