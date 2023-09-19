using Byces.Calculator.Enums;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Byces.Calculator.Representations
{
    internal abstract class ExpressionRepresentation<T> : Representable 
        where T : ExpressionRepresentation<T>
    {
        private const int StringSizeLimit = 128;

        static ExpressionRepresentation()
        {
            Type mainType = typeof(ExpressionRepresentation<T>);
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

        protected ExpressionRepresentation(ExpressionConflict conflicts, ExpressionConflict representationConflict) : base(_items.Length)
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();
            bool charIsDefault = CharRepresentation == '\0';

            if (stringIsDefault && charIsDefault)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has no representation");
            if (!charIsDefault && (char.IsWhiteSpace(CharRepresentation) || char.IsDigit(CharRepresentation) || CharRepresentation == '(' || CharRepresentation == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation with an illegal character.");
            if (!stringIsDefault && spanRepresentation.Length == 1)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with length 1.");
            if (!stringIsDefault && spanRepresentation.Length > StringSizeLimit)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation above the allowed limit of {StringSizeLimit}.");
            if (!stringIsDefault && spanRepresentation.Any(x => char.IsWhiteSpace(x) || char.IsDigit(x) || x == '(' || x == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with illegal characters.");
            for (int i = 0; i < 3; i++)
            {
                var conflictToCheckFlag = (ExpressionConflict)Math.Pow(2, i);
                if ((conflicts & conflictToCheckFlag) != conflictToCheckFlag) continue;
                Type conflictType = conflictToCheckFlag switch
                {
                    ExpressionConflict.Variable => typeof(ExpressionRepresentation<VariableRepresentation>),
                    ExpressionConflict.Function => typeof(ExpressionRepresentation<FunctionRepresentation>),
                    ExpressionConflict.Operator => typeof(ExpressionRepresentation<OperatorRepresentation>),
                    _ => throw new NotImplementedException(),
                };
                FieldInfo itemsFieldInfo = conflictType.GetField(nameof(_items), BindingFlags.NonPublic | BindingFlags.Static)!;
                Representable[] itemsField = (Representable[])itemsFieldInfo.GetValue(null)!;

                for (int j = 0; j < itemsField.Length; j++)
                {
                    ReadOnlySpan<char> itemSpanRepresentation = itemsField[j].StringRepresentation;

                    if (!charIsDefault && char.ToUpper(itemsField[j].CharRepresentation) == char.ToUpper(CharRepresentation))
                        throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation identical to another type.");
                    if (!stringIsDefault && spanRepresentation.Equals(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                        throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation identical to another type.");
                    if (!stringIsDefault && itemSpanRepresentation.StartsWith(spanRepresentation, StringComparison.OrdinalIgnoreCase))
                    {
                        int diff = 0;
                        for (int l = 0; l < itemSpanRepresentation.Length; l++)
                        {
                            if (l < spanRepresentation.Length && itemSpanRepresentation[l] == spanRepresentation[l]) continue;
                            diff = itemSpanRepresentation.Length - l; break;
                        }
                        Array.Resize(ref RepresentableConflicts, RepresentableConflicts.Length + 1);
                        RepresentableConflicts[^1] = new Conflict(diff, conflictToCheckFlag, RepresentableType.String);
                    }
                    else if (!stringIsDefault && !itemSpanRepresentation.IsEmpty && spanRepresentation.StartsWith(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                    {
                        int diff = 0;
                        for (int l = 0; l < spanRepresentation.Length; l++)
                        {
                            if (l < itemSpanRepresentation.Length && itemSpanRepresentation[l] == spanRepresentation[l]) continue;
                            diff = spanRepresentation.Length - l; break;
                        }
                        Array.Resize(ref itemsField[j].RepresentableConflicts, itemsField[j].RepresentableConflicts.Length + 1);
                        itemsField[j].RepresentableConflicts[^1] = new Conflict(diff, representationConflict, RepresentableType.String);
                    }
                    if (!charIsDefault && !itemSpanRepresentation.IsEmpty && char.ToUpper(itemSpanRepresentation[0]) == char.ToUpper(CharRepresentation))
                    {
                        int diff = itemSpanRepresentation.Length - 1;
                        Array.Resize(ref RepresentableConflicts, RepresentableConflicts.Length + 1);
                        RepresentableConflicts[^1] = new Conflict(diff, conflictToCheckFlag, RepresentableType.Char);
                    }
                    if (!spanRepresentation.IsEmpty && char.ToUpper(spanRepresentation[0]) == char.ToUpper(itemsField[j].CharRepresentation))
                    {
                        int diff = spanRepresentation.Length - 1;
                        Array.Resize(ref itemsField[j].RepresentableConflicts, itemsField[j].RepresentableConflicts.Length + 1);
                        itemsField[j].RepresentableConflicts[^1] = new Conflict(diff, representationConflict, RepresentableType.Char);
                    }
                }
            }
            if (!charIsDefault && !stringIsDefault && char.ToUpper(spanRepresentation[0]) == char.ToUpper(CharRepresentation))
            {
                int diff = spanRepresentation.Length - 1;
                Array.Resize(ref RepresentableConflicts, RepresentableConflicts.Length + 1);
                RepresentableConflicts[^1] = new Conflict(diff, representationConflict, RepresentableType.Char);
            }
            if (!stringIsDefault && spanRepresentation.Length > MaxStringSize) MaxStringSize = spanRepresentation.Length;
            Array.Resize(ref _items, _items.Length + 1);
            _items[^1] = (T)this;

            if (!stringIsDefault) StringToType.Add(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase), _items[^1]);
            if (charIsDefault) return;
            if (char.ToLower(CharRepresentation) != char.ToUpper(CharRepresentation))
            {
                CharToType.Add(char.ToUpper(CharRepresentation), _items[^1]);
                CharToType.Add(char.ToLower(CharRepresentation), _items[^1]);
            }
            else
            {
                CharToType.Add(CharRepresentation, _items[^1]);
            }
        }

        private static T[] _items = Array.Empty<T>();
        
        private static readonly Dictionary<int, T> StringToType = new();
        private static readonly Dictionary<char, T> CharToType = new();

        internal static int MaxStringSize { get; private set; }
        
        internal static T Parse(ReadOnlySpan<char> span)
        {
            return span.Length == 1 
                ? CharToType[span[0]] 
                : StringToType[string.GetHashCode(span, StringComparison.OrdinalIgnoreCase)];
        }

        internal static bool TryParse(ReadOnlySpan<char> span, out T type)
        {
            return span.Length == 1 
                ? CharToType.TryGetValue(span[0], out type!) 
                : StringToType.TryGetValue(string.GetHashCode(span, StringComparison.OrdinalIgnoreCase), out type!);
        }

        internal static T GetItem(int value) => _items[value];
    }
}