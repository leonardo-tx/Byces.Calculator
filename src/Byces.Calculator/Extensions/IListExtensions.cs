using System.Collections.Generic;

namespace Byces.Calculator.Extensions
{
    internal static class IListExtensions
    {
        internal static int CustomMax(this IList<int> source)
        {
            int max = 0;

            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] > max) max = source[i];
            }
            return max;
        }
    }
}