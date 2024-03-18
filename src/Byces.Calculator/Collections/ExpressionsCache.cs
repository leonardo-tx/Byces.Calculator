using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Collections
{
    internal sealed class ExpressionsCache
    {
        private readonly List<KeyValuePair<string, CachedContent>> _items = new();

        public int Count
        {
            get
            {
                lock (_items)
                {
                    return _items.Count;
                }
            }
        }
        
        public void Free()
        {
            lock (_items)
            {
                _items.Clear();
                _items.TrimExcess();
            }
        }

        public void Add(ReadOnlySpan<char> expression, CachedContent cachedContent)
        {
            lock (_items)
            {
                int index = GetIndexToInsert(expression);
                if (index == -1) return;
                
                _items.Insert(index, new KeyValuePair<string, CachedContent>(expression.ToString(), cachedContent));
            }
        }

        private int GetIndexToInsert(ReadOnlySpan<char> expression)
        {
            ReadOnlySpan<KeyValuePair<string, CachedContent>> span = CollectionsMarshal.AsSpan(_items);
            int left = 0, right = span.Length - 1;
            
            while (left <= right)
            {
                int index = left + (right - left >> 1);
                string key = span[index].Key;

                int compareResult = expression.CompareTo(key, StringComparison.OrdinalIgnoreCase);
                if (compareResult == 0)
                {
                    return -1;
                }
                if (compareResult < 0)
                {
                    right = index - 1;
                    continue;
                }
                left = index + 1;
            }
            return left;
        }

        public bool TryGetValue(ReadOnlySpan<char> expression, [NotNullWhen(true)] out CachedContent? cachedContent)
        {
            lock (_items)
            {
                int index = GetIndexOf(expression);
                if (index == -1)
                {
                    cachedContent = null;
                    return false;
                }
                cachedContent = _items[index].Value;
                return true;
            }
        }

        private int GetIndexOf(ReadOnlySpan<char> s)
        {
            ReadOnlySpan<KeyValuePair<string, CachedContent>> span = CollectionsMarshal.AsSpan(_items);
            int left = 0, right = span.Length - 1;
            
            while (left <= right)
            {
                int index = left + (right - left >> 1);
                string key = span[index].Key;

                int compareResult = s.CompareTo(key, StringComparison.OrdinalIgnoreCase);
                if (compareResult == 0)
                {
                    return index;
                }
                if (compareResult < 0)
                {
                    right = index - 1;
                    continue;
                }
                left = index + 1;
            }
            return -1;
        }
    }
}