using System;
using System.Reflection;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal sealed class BuiltExpressions
    {
        public BuiltExpressions(params Assembly[] assemblies)
        {
            Type afterRepresentationType = typeof(ExpressionItem<OperatorItem>);
            Type beforeRepresentationType = typeof(ExpressionItem<BeforeVariableItem>);
            _conflictsItems = new object[] { BeforeConflictItems, AfterConflictItems };
            
            foreach (Assembly assembly in assemblies)
            {
                ReadOnlySpan<Type> libraryTypes = assembly.GetTypes();
                for (int i = 0; i < libraryTypes.Length; i++)
                {
                    Type? baseType = libraryTypes[i].BaseType;
                    if (baseType == null) continue;

                    if (libraryTypes[i].IsAbstract || (!baseType.IsSubclassOf(afterRepresentationType) && !baseType.IsSubclassOf(beforeRepresentationType))) continue;
                    object instance = Activator.CreateInstance(libraryTypes[i])!;
                    
                    if (instance is BeforeVariableItem beforeVariable) 
                        AddRepresentation(beforeVariable, 0);
                    else if (instance is OperatorItem afterVariable)
                        AddRepresentation(afterVariable, 1);
                }
            }
        }

        private void AddRepresentation<T>(T instance, int index) where T : ExpressionItem<T>
        {
            ConflictItems<T> conflictItems = (ConflictItems<T>)_conflictsItems[index];
            T[] items = conflictItems.Items;
            
            ReadOnlySpan<char> spanRepresentation = instance.StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();
            bool charIsDefault = instance.CharRepresentation == '\0';
            
            if (!charIsDefault && conflictItems.CharToType.ContainsKey(instance.CharRepresentation))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation identical to another type.");
            if (!stringIsDefault && conflictItems.StringToType.ContainsKey(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation identical to another type.");
            
            for (int i = 0; i < items.Length; i++)
            {
                ReadOnlySpan<char> itemSpanRepresentation = items[i].StringRepresentation;
                
                if (!stringIsDefault && itemSpanRepresentation.StartsWith(spanRepresentation, StringComparison.OrdinalIgnoreCase))
                {
                    int diff = 0;
                    for (int j = 0; j < itemSpanRepresentation.Length; j++)
                    {
                        if (j < spanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                        diff = itemSpanRepresentation.Length - j; break;
                    }
                    Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                    instance.RepresentableConflicts[^1] = new Conflict(diff, RepresentableType.String);
                }
                else if (!stringIsDefault && !itemSpanRepresentation.IsEmpty && spanRepresentation.StartsWith(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                {
                    int diff = 0;
                    for (int j = 0; j < spanRepresentation.Length; j++)
                    {
                        if (j < itemSpanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                        diff = spanRepresentation.Length - j; break;
                    }
                    Array.Resize(ref items[i].RepresentableConflicts, items[i].RepresentableConflicts.Length + 1);
                    items[i].RepresentableConflicts[^1] = new Conflict(diff, RepresentableType.String);
                }
                if (!charIsDefault && !itemSpanRepresentation.IsEmpty && char.ToUpper(itemSpanRepresentation[0]) == char.ToUpper(instance.CharRepresentation))
                {
                    int diff = itemSpanRepresentation.Length - 1;
                    Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                    instance.RepresentableConflicts[^1] = new Conflict(diff, RepresentableType.Char);
                }
                if (!spanRepresentation.IsEmpty && char.ToUpper(spanRepresentation[0]) == char.ToUpper(items[i].CharRepresentation))
                {
                    int diff = spanRepresentation.Length - 1;
                    Array.Resize(ref items[i].RepresentableConflicts, items[i].RepresentableConflicts.Length + 1);
                    items[i].RepresentableConflicts[^1] = new Conflict(diff, RepresentableType.Char);
                }
            }
            if (!charIsDefault && !stringIsDefault && char.ToUpper(spanRepresentation[0]) == char.ToUpper(instance.CharRepresentation))
            {
                int diff = spanRepresentation.Length - 1;
                Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                instance.RepresentableConflicts[^1] = new Conflict(diff, RepresentableType.Char);
            }

            conflictItems.AddToItemsArray(instance);
            items = conflictItems.Items;

            if (!stringIsDefault) conflictItems.StringToType.Add(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase), items[^1]);
            if (charIsDefault) return;
            if (char.ToLower(instance.CharRepresentation) != char.ToUpper(instance.CharRepresentation))
            {
                conflictItems.CharToType.Add(char.ToUpper(instance.CharRepresentation), items[^1]);
                conflictItems.CharToType.Add(char.ToLower(instance.CharRepresentation), items[^1]);
            }
            else
            {
                conflictItems.CharToType.Add(instance.CharRepresentation, items[^1]);
            }
        }

        internal readonly ConflictItems<BeforeVariableItem> BeforeConflictItems = new();
        internal readonly ConflictItems<OperatorItem> AfterConflictItems = new();

        private readonly object[] _conflictsItems;
    }
}