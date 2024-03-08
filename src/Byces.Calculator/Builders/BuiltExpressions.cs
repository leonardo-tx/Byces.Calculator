using System;
using System.Reflection;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions.Items;

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
            conflictItems.AddItem(instance);
        }

        internal readonly ConflictItems<BeforeVariableItem> BeforeConflictItems = new();
        internal readonly ConflictItems<OperatorItem> AfterConflictItems = new();

        private readonly object[] _conflictsItems;
    }
}