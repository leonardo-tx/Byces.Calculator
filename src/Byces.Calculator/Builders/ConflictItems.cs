using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal sealed class ConflictItems<T> where T : ExpressionItem<T>
    {
        internal T[] Items = Array.Empty<T>();
        
        internal readonly Dictionary<int, T> StringToType = new();
        internal readonly Dictionary<char, T> CharToType = new();

        internal void AddToItemsArray(T instance)
        {
            Array.Resize(ref Items, Items.Length + 1);
            Items[^1] = instance;
        }
        
        internal T Parse(ReadOnlySpan<char> span)
        {
            return span.Length == 1 
                ? CharToType[span[0]] 
                : StringToType[string.GetHashCode(span, StringComparison.OrdinalIgnoreCase)];
        }
        
        internal bool TryParse(ReadOnlySpan<char> span, [NotNullWhen(true)] out T? type)
        {
            return span.Length == 1 
                ? CharToType.TryGetValue(span[0], out type) 
                : StringToType.TryGetValue(string.GetHashCode(span, StringComparison.OrdinalIgnoreCase), out type);
        }
    }
}