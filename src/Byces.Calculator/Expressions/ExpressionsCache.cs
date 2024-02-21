using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Byces.Calculator.Expressions
{
    internal sealed class ExpressionsCache
    {
        private readonly Dictionary<int, Content> _cachedExpressions = new();

        public int Count
        {
            get
            {
                lock (_cachedExpressions)
                {
                    return _cachedExpressions.Count;
                }
            }
        }

        public void Add(ReadOnlySpan<char> expression, Content content)
        {
            int hashCode = string.GetHashCode(expression, StringComparison.OrdinalIgnoreCase);
            lock (_cachedExpressions)
            {
                _cachedExpressions.Add(hashCode, content);
            }
        }

        public bool TryGetContent(ReadOnlySpan<char> expression, [NotNullWhen(true)] out Content? content)
        {
            int hashCode = string.GetHashCode(expression, StringComparison.OrdinalIgnoreCase);
            lock (_cachedExpressions)
            {
                return _cachedExpressions.TryGetValue(hashCode, out content);
            }
        }

        public void Free()
        {
            lock (_cachedExpressions)
            {
                _cachedExpressions.Clear();
                _cachedExpressions.TrimExcess();
            }
        }
    }
}