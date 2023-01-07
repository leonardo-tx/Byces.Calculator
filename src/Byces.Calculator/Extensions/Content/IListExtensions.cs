using Byces.Calculator.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Extensions.Content
{
    internal static class IListExtensions
    {
        /// <summary>
        /// Returns the maximum priority of an <see cref="Operation"/> list.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The maximum priority in the sequence, or -1 if source is empty.</returns>
        internal static int MaxPriority(this IList<Operation> source)
        {
            int max = -1;
#if NET5_0_OR_GREATER
            ReadOnlySpan<Operation> reference = CollectionsMarshal.AsSpan((List<Operation>)source);
            for (int i = 0; i < reference.Length; i++)
            {
                if (reference[i].Priority > max) max = reference[i].Priority;
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