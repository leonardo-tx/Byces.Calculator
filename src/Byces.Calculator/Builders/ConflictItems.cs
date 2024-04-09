using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Byces.Calculator.Collections;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal class ConflictItems<T> where T : ExpressionItem<T>
    {
        private readonly CharSpanDictionary<T> _itemsSpan = new();
        private readonly Dictionary<char, T> _itemsChar = new();

        protected void AddItem(T item, List<T> tempItems)
        {
            tempItems.Add(item);
            AddCollisions(item, tempItems);
            foreach (string representation in item.StringRepresentations)
            {
                if (representation.Length > 1)
                {
                    if (_itemsSpan.Add(representation, item)) continue;
                    throw new ArgumentException($"Unable to initialize the type. The {item.GetType().FullName} class has a string representation that collides with another.");
                }
                char lowerVariant = char.ToLower(representation[0]);
                char upperVariant = char.ToUpper(representation[0]);

                if (lowerVariant != upperVariant)
                {
                    if (!_itemsChar.TryAdd(lowerVariant, item)) 
                        throw new ArgumentException($"Unable to initialize the type. The {item.GetType().FullName} class has a string representation that collides with another.");
                }
                if (!_itemsChar.TryAdd(upperVariant, item))
                    throw new ArgumentException($"Unable to initialize the type. The {item.GetType().FullName} class has a string representation that collides with another.");
            }
        }
        
        private void AddCollisions(T item, List<T> tempItems)
        {
            int indexForSameInstance = 1;
            foreach (string stringRepresentation in item.StringRepresentations)
            {
                ReadOnlySpan<char> spanRepresentation = stringRepresentation;
                foreach (T anotherItem in tempItems)
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
            return span.Length == 1 ? _itemsChar[span[0]] : _itemsSpan[span];
        }
        
        internal bool TryParse(ReadOnlySpan<char> span, [NotNullWhen(true)] out T? type)
        {
            return span.Length == 1 ? _itemsChar.TryGetValue(span[0], out type) : _itemsSpan.TryGetValue(span, out type);
        }
    }
}