using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal sealed class ConflictItems<T> where T : ExpressionItem<T>
    {
        internal T[] Items = Array.Empty<T>();
        
        internal readonly Dictionary<int, T> StringToItems = new();

        internal void AddToItemsArray(T instance)
        {
            Array.Resize(ref Items, Items.Length + 1);
            Items[^1] = instance;
        }
        
        internal T Parse(ReadOnlySpan<char> span)
        {
            return StringToItems[string.GetHashCode(span, StringComparison.OrdinalIgnoreCase)];
        }
        
        internal bool TryParse(ReadOnlySpan<char> span, [NotNullWhen(true)] out T? type)
        {
            return StringToItems.TryGetValue(string.GetHashCode(span, StringComparison.OrdinalIgnoreCase), out type);
        }
    }
}