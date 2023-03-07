using System;
using System.Collections.Generic;
using System.Reflection;

namespace Byces.Calculator.Enums
{
    internal abstract class ExpressionType<T> where T : ExpressionType<T>
    {
        private const int StringSizeLimit = 128;

        static ExpressionType()
        {
            Type mainType = typeof(ExpressionType<T>);
            Assembly libraryAssembly = mainType.Assembly;

            ReadOnlySpan<Type> libraryTypes = libraryAssembly.GetTypes();
            for (int i = 0; i < libraryTypes.Length; i++)
            {
                Type? baseType = libraryTypes[i].BaseType;
                if (baseType == null) continue;

                if (libraryTypes[i].IsAbstract || !baseType.IsSubclassOf(mainType)) continue;
                Activator.CreateInstance(libraryTypes[i]);
            }
        }

        protected ExpressionType()
        {
            Value = _items.Length;
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();
            bool charIsDefault = CharRepresentation == '\0';
            for (int i = 0; i < _items.Length; i++)
            {
                ReadOnlySpan<char> itemSpanRepresentation = _items[i].StringRepresentation;
                if (!charIsDefault && _items[i].CharRepresentation == CharRepresentation)
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation identical to another type.");
                if (!charIsDefault && char.IsWhiteSpace(CharRepresentation))
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a whitespace char representation.");
                if (!stringIsDefault && spanRepresentation.Length > StringSizeLimit)
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation above the allowed limit of {StringSizeLimit}");
                if (!stringIsDefault && spanRepresentation.Equals(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation identical to another type.");
                
                if (!stringIsDefault)
                {
                    if (itemSpanRepresentation.StartsWith(spanRepresentation))
                    {
                        int additionalCheck = 0;
                        for (int j = 0; j < itemSpanRepresentation.Length; j++)
                        {
                            if (j < spanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                            additionalCheck = itemSpanRepresentation.Length - j; break;
                        }
                        if (additionalCheck > AdditionalCheck) AdditionalCheck = additionalCheck;
                    }
                    else if (spanRepresentation.StartsWith(itemSpanRepresentation))
                    {
                        int additionalCheck = 0;
                        for (int j = 0; j < spanRepresentation.Length; j++)
                        {
                            if (j < itemSpanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                            additionalCheck = spanRepresentation.Length - j; break;
                        }
                        if (additionalCheck > _items[i].AdditionalCheck) _items[i].AdditionalCheck = additionalCheck;
                    }
                }
                if (!charIsDefault)
                {
                    if (!itemSpanRepresentation.IsEmpty && itemSpanRepresentation[0] == CharRepresentation)
                    {
                        int additionalCheck = itemSpanRepresentation.Length - 1;
                        if (additionalCheck > AdditionalCheck) AdditionalCheck = additionalCheck;
                    }
                    else if (!spanRepresentation.IsEmpty && spanRepresentation[0] == _items[i].CharRepresentation)
                    {
                        int additionalCheck = spanRepresentation.Length - 1;
                        if (additionalCheck > _items[i].AdditionalCheck) _items[i].AdditionalCheck = additionalCheck;
                    }
                }
            }
            if (spanRepresentation.Length > MaxStringSize) MaxStringSize = spanRepresentation.Length;

            T[] updatedItems = new T[_items.Length + 1];
            Array.Copy(_items, updatedItems, _items.Length);

            updatedItems[^1] = (T)this;
#if NETCOREAPP3_0_OR_GREATER
            if (!stringIsDefault) _stringToType.Add(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase), updatedItems[^1]);
            if (!charIsDefault) _charToType.Add(CharRepresentation.GetHashCode(), updatedItems[^1]);
#endif
            _items = updatedItems;
        }

        private static T[] _items = Array.Empty<T>();

#if NETCOREAPP3_0_OR_GREATER
        private static readonly Dictionary<int, T> _stringToType = new Dictionary<int, T>();

        private static readonly Dictionary<int, T> _charToType = new Dictionary<int, T>();
#endif

        internal int Value { get; }

        internal int AdditionalCheck { get; private set; }

        internal static int MaxStringSize { get; private set; }

        protected virtual string StringRepresentation { get; } = string.Empty;

        protected virtual char CharRepresentation { get; } = '\0';

        public abstract ResultType ResultType { get; }

#if NETCOREAPP3_0_OR_GREATER
        internal static bool TryParse(ReadOnlySpan<char> span, out T type)
        {
            if (span.Length == 1) return TryParse(span[0], out type);
            return _stringToType.TryGetValue(string.GetHashCode(span, StringComparison.OrdinalIgnoreCase), out type!);
        }

        internal static bool TryParse(char character, out T type) => _charToType.TryGetValue(character.GetHashCode(), out type!);
#else
        internal static bool TryParse(ReadOnlySpan<char> span, out T type)
        {
            if (span.Length == 1) return TryParse(span[0], out type);

            ReadOnlySpan<T> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                type = reference[i]; return true;
            }
            type = default!; return false;
        }

        internal static bool TryParse(char character, out T type)
        {
            if (character == '\0') { type = default!; return false; }

            ReadOnlySpan<T> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                type = reference[i]; return true;
            }
            type = default!; return false;
        }
#endif

        internal static T GetItem(int value) => _items[value];
    }
}