using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal sealed class ConflictItems<T> where T : ExpressionItem<T>
    {
        private readonly HashSet<T> _uniqueItems = new();

        private int _count;

        private KeyValuePair<string, T>?[] _items = new KeyValuePair<string, T>?[1];

        internal void AddItem(T item)
        {
            if (!_uniqueItems.Add(item)) return;

            int requiredLength = _count + item.StringRepresentations.Length;
            IncreaseInternalArraySize(requiredLength);
            AddCollisions(item);
            
            foreach (string representation in item.StringRepresentations)
            {
                AddPair(representation, item);
            }
        }

        private void AddPair(string representation, T item)
        {
            int index = GetIndex(representation);
            while (_items[index] != null)
            {
                if (representation.Equals(_items[index]!.Value.Key, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException($"Unable to initialize the type. The {item.GetType().FullName} class has a string representation that collides with another.");

                index = (index + 1) % _items.Length;
            }
            _items[index] = new KeyValuePair<string, T>(representation, item);
            _count += 1;
        }

        private void AddCollisions(T item)
        {
            int indexForSameInstance = 1;
            foreach (string stringRepresentation in item.StringRepresentations)
            {
                ReadOnlySpan<char> spanRepresentation = stringRepresentation;
                foreach (T anotherItem in _uniqueItems)
                {
                    bool sameInstance = item == anotherItem;
                    for (int i = sameInstance ? indexForSameInstance : 0; i < anotherItem.StringRepresentations.Length; i++)
                    {
                        ReadOnlySpan<char> itemSpanRepresentation = anotherItem.StringRepresentations[i];
                        if (itemSpanRepresentation.StartsWith(spanRepresentation, StringComparison.OrdinalIgnoreCase))
                        {
                            int diff = 0;
                            for (int j = 0; j < itemSpanRepresentation.Length; j++)
                            {
                                if (j < spanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                                diff = itemSpanRepresentation.Length - j; break;
                            }
                            Array.Resize(ref item.RepresentableConflicts, item.RepresentableConflicts.Length + 1);
                            item.RepresentableConflicts[^1] = new Conflict(diff);
                        }
                        else if (spanRepresentation.StartsWith(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                        {
                            int diff = 0;
                            for (int j = 0; j < spanRepresentation.Length; j++)
                            {
                                if (j < itemSpanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                                diff = spanRepresentation.Length - j; break;
                            }
                            Array.Resize(ref anotherItem.RepresentableConflicts, anotherItem.RepresentableConflicts.Length + 1);
                            anotherItem.RepresentableConflicts[^1] = new Conflict(diff);
                        }
                    }
                }
                indexForSameInstance += 1;
            }
        }
        
        internal T Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out T? type)) return type;
            throw new ArgumentException();
        }
        
        internal bool TryParse(ReadOnlySpan<char> span, [NotNullWhen(true)] out T? type)
        {
            int index = GetIndex(span);
            while (_items[index] != null)
            {
                if (span.Equals(_items[index]!.Value.Key, StringComparison.OrdinalIgnoreCase))
                {
                    type = _items[index]!.Value.Value;
                    return true;
                }
                index = (index + 1) % _items.Length;
            }
            type = null;
            return false;
        }

        private void IncreaseInternalArraySize(int requiredLength)
        {
            int itemsLength = _items.Length;
            while (requiredLength > itemsLength * 0.75) itemsLength *= 2;

            if (itemsLength == _items.Length) return;
            
            KeyValuePair<string, T>?[] oldItems = _items;
            _items = new KeyValuePair<string, T>?[itemsLength];

            foreach (KeyValuePair<string, T>? oldItem in oldItems)
            {
                if (oldItem == null) continue;
                
                int index = GetIndex(oldItem.Value.Key);
                while (_items[index] != null)
                {
                    index = (index + 1) % _items.Length;
                }
                _items[index] = oldItem;
            }
        }

        private int GetIndex(ReadOnlySpan<char> s)
        {
            return Math.Abs(string.GetHashCode(s, StringComparison.OrdinalIgnoreCase) % _items.Length);
        }
    }
}