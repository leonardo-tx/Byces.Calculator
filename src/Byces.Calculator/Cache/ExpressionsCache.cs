using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Byces.Calculator.Cache
{
    internal sealed class ExpressionsCache
    {
        private readonly Dictionary<int, CachedContent> _cachedExpressions = new();

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

        public void Add(ReadOnlySpan<char> expression, CachedContent cachedContent)
        {
            int hashCode = string.GetHashCode(expression, StringComparison.OrdinalIgnoreCase);
            lock (_cachedExpressions)
            {
                _cachedExpressions.TryAdd(hashCode, cachedContent);
            }
        }

        public bool TryGetContent(ReadOnlySpan<char> expression, [NotNullWhen(true)] out CachedContent? cachedContent)
        {
            int hashCode = string.GetHashCode(expression, StringComparison.OrdinalIgnoreCase);
            lock (_cachedExpressions)
            {
                return _cachedExpressions.TryGetValue(hashCode, out cachedContent);
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