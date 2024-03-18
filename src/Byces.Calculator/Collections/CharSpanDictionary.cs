using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Byces.Calculator.Collections
{
    internal sealed class CharSpanDictionary<TValue>
    {
        private const double LoadFactor = 0.75;
        private const int InitialCapacity = 16;

        public CharSpanDictionary()
        {
            _items = new IList<KeyValuePair<string, TValue>>[InitialCapacity];
            Array.Fill(_items, Array.Empty<KeyValuePair<string, TValue>>());
        }

        private IList<KeyValuePair<string, TValue>>[] _items;
        private int _count;

        public bool Add(string key, TValue value)
        {
            EnsureCapacity();
            int index = GetIndex(key);

            if (_items[index].Count == 0)
            {
                _items[index] = new List<KeyValuePair<string, TValue>> { new(key, value) };
            }
            else
            {
                if (_items[index].Any(pair => key.Equals(pair.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
                _items[index].Add(new KeyValuePair<string, TValue>(key, value));
            }
            _count += 1;
            return true;
        }

        public TValue this[ReadOnlySpan<char> s]
        {
            get
            {
                if (TryGetValue(s, out TValue? value)) return value;
                throw new ArgumentException("The specified key does not exist in the collection");
            }
        }

        public bool TryGetValue(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out TValue value)
        {
            int index = GetIndex(s);
            if (_items[index].Count == 0)
            {
                value = default;
                return false;
            }
            IList<KeyValuePair<string, TValue>> pairs = _items[index];
            for (int i = 0; i < pairs.Count; i++)
            {
                KeyValuePair<string, TValue> pair = pairs[i];
                if (s.Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                {
                    value = pair.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        private void EnsureCapacity()
        {
            if (_count + 1 <= _items.Length * LoadFactor) return;
            
            IList<KeyValuePair<string, TValue>>[] oldItems = _items;
            _items = new IList<KeyValuePair<string, TValue>>[_items.Length * 2];
            Array.Fill(_items, Array.Empty<KeyValuePair<string, TValue>>());

            foreach (IList<KeyValuePair<string, TValue>> oldItem in oldItems)
            {
                foreach (KeyValuePair<string, TValue> pair in oldItem)
                {
                    int index = GetIndex(pair.Key);
                    if (_items[index].Count == 0)
                    {
                        _items[index] = new List<KeyValuePair<string, TValue>> { pair };
                        continue;
                    }
                    _items[index].Add(pair);
                }
            }
        }
        
        private int GetIndex(ReadOnlySpan<char> s)
        {
            int hashCode = string.GetHashCode(s, StringComparison.OrdinalIgnoreCase);
            return Math.Abs(hashCode % _items.Length);
        }
    }
}