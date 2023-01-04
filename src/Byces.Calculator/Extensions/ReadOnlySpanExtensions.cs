using System;

namespace Byces.Calculator.Extensions
{
    internal static class ReadOnlySpanExtensions
    {
        internal static int Count(this ReadOnlySpan<char> expressionSpan, ReadOnlySpan<char> characters)
        {
            int count = 0;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (expressionSpan[i] == characters[j]) count++;
                }
            }
            return count;
        }
    }
}
