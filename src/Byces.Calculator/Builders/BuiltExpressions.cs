using System;
using System.Reflection;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions.Items;
using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Builders
{
    internal sealed class BuiltExpressions
    {
        private static readonly Type FunctionType = typeof(FunctionItem);
        private static readonly Type OperatorType = typeof(OperatorItem);
        private static readonly Type VariableType = typeof(VariableItem);
        
        public BuiltExpressions(CalculatorOptions options, Assembly[] assemblies)
        {
            _conflictsItems = new object[] { BeforeConflictItems, AfterConflictItems };
            
            InstantiateExpressionItemsInAssembly(options, FunctionType.Assembly);
            foreach (Assembly assembly in assemblies)
            {
                InstantiateExpressionItemsInAssembly(CalculatorOptions.Default, assembly);
            }
        }

        private void InstantiateExpressionItemsInAssembly(CalculatorOptions options, Assembly assembly)
        {
            ReadOnlySpan<Type> libraryTypes = assembly.GetTypes();
            foreach (Type type in libraryTypes)
            {
                if (type.IsAbstract) continue;
                if ((options & CalculatorOptions.RemoveDefaultFunctions) == 0 && type.IsSubclassOf(FunctionType) ||
                    (options & CalculatorOptions.RemoveDefaultVariables) == 0 && type.IsSubclassOf(VariableType))
                {
                    BeforeVariableItem instance = (BeforeVariableItem)Activator.CreateInstance(type)!;
                    AddRepresentation(instance, 0);

                    continue;
                }
                if (type.IsSubclassOf(OperatorType))
                {
                    OperatorItem instance = (OperatorItem)Activator.CreateInstance(type)!;
                    AddRepresentation(instance, 1);
                }
            }
        }

        private void AddRepresentation<T>(T instance, int conflictIndex) where T : ExpressionItem<T>
        {
            ConflictItems<T> conflictItems = (ConflictItems<T>)_conflictsItems[conflictIndex];
            T[] items = conflictItems.Items;

            int index = 0;
            foreach (string stringRepresentation in instance.StringRepresentations)
            {
                ReadOnlySpan<char> spanRepresentation = stringRepresentation;
                if (conflictItems.StringToItems.ContainsKey(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase)))
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation identical to another type.");
                
                for (int i = 0; i < items.Length; i++)
                {
                    foreach (string itemStringRepresentation in items[i].StringRepresentations)
                    {
                        ReadOnlySpan<char> itemSpanRepresentation = itemStringRepresentation;
                        if (itemSpanRepresentation.StartsWith(spanRepresentation, StringComparison.OrdinalIgnoreCase))
                        {
                            int diff = 0;
                            for (int j = 0; j < itemSpanRepresentation.Length; j++)
                            {
                                if (j < spanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                                diff = itemSpanRepresentation.Length - j; break;
                            }
                            Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                            instance.RepresentableConflicts[^1] = new Conflict(diff);
                        }
                        else if (spanRepresentation.StartsWith(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                        {
                            int diff = 0;
                            for (int j = 0; j < spanRepresentation.Length; j++)
                            {
                                if (j < itemSpanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                                diff = spanRepresentation.Length - j; break;
                            }
                            Array.Resize(ref items[i].RepresentableConflicts, items[i].RepresentableConflicts.Length + 1);
                            items[i].RepresentableConflicts[^1] = new Conflict(diff);
                        }
                    }
                }
                for (int i = index + 1; i < instance.StringRepresentations.Length; i++)
                {
                    ReadOnlySpan<char> itemSpanRepresentation = instance.StringRepresentations[i];
                    if (itemSpanRepresentation.StartsWith(spanRepresentation, StringComparison.OrdinalIgnoreCase))
                    {
                        int diff = 0;
                        for (int j = 0; j < itemSpanRepresentation.Length; j++)
                        {
                            if (j < spanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                            diff = itemSpanRepresentation.Length - j; break;
                        }
                        Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                        instance.RepresentableConflicts[^1] = new Conflict(diff);
                    }
                    else if (spanRepresentation.StartsWith(itemSpanRepresentation, StringComparison.OrdinalIgnoreCase))
                    {
                        int diff = 0;
                        for (int j = 0; j < spanRepresentation.Length; j++)
                        {
                            if (j < itemSpanRepresentation.Length && itemSpanRepresentation[j] == spanRepresentation[j]) continue;
                            diff = spanRepresentation.Length - j; break;
                        }
                        Array.Resize(ref instance.RepresentableConflicts, instance.RepresentableConflicts.Length + 1);
                        instance.RepresentableConflicts[^1] = new Conflict(diff);
                    }
                }
                conflictItems.StringToItems.Add(string.GetHashCode(spanRepresentation, StringComparison.OrdinalIgnoreCase), instance);
                index++;
            }
            conflictItems.AddToItemsArray(instance);
        }

        internal readonly ConflictItems<BeforeVariableItem> BeforeConflictItems = new();
        internal readonly ConflictItems<OperatorItem> AfterConflictItems = new();

        private readonly object[] _conflictsItems;
    }
}