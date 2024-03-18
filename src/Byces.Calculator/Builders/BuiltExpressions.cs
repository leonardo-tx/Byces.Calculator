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
            InstantiateExpressionItemsInAssembly(options, FunctionType.Assembly);
            foreach (Assembly assembly in assemblies)
            {
                InstantiateExpressionItemsInAssembly(CalculatorOptions.Default, assembly);
            }
            foreach (BeforeVariableItem item in BeforeConflictItems.UniqueItems)
            {
                item.StringRepresentations = Array.Empty<string>();
            }
            foreach (OperatorItem item in AfterConflictItems.UniqueItems)
            {
                item.StringRepresentations = Array.Empty<string>();
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
                    BeforeConflictItems.AddItem(instance);

                    continue;
                }
                if (type.IsSubclassOf(OperatorType))
                {
                    OperatorItem instance = (OperatorItem)Activator.CreateInstance(type)!;
                    AfterConflictItems.AddItem(instance);
                }
            }
        }

        internal readonly ConflictItems<BeforeVariableItem> BeforeConflictItems = new();
        internal readonly ConflictItems<OperatorItem> AfterConflictItems = new();
    }
}