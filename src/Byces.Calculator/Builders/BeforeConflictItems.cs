using System;
using System.Collections.Generic;
using System.Reflection;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions.Items;
using Microsoft.Extensions.DependencyInjection;

namespace Byces.Calculator.Builders
{
    internal sealed class BeforeConflictItems : ConflictItems<BeforeVariableItem>
    {
        private static readonly Type FunctionType = typeof(FunctionItem);
        private static readonly Type VariableType = typeof(VariableItem);

        public BeforeConflictItems(Assembly libraryAssembly, Assembly[] customAssemblies, IServiceProvider? serviceProvider, CalculatorOptions options)
        {
            List<BeforeVariableItem> tempItems = new();
            InstantiateItemsInAssembly(libraryAssembly, tempItems, options);

            if (serviceProvider == null)
            {
                foreach (Assembly assembly in customAssemblies)
                {
                    InstantiateItemsInAssembly(assembly, tempItems, CalculatorOptions.Default);
                }
            }
            else
            {
                foreach (Assembly assembly in customAssemblies)
                {
                    InstantiateItemsInAssembly(assembly, tempItems, CalculatorOptions.Default, serviceProvider);
                }
            }
            foreach (BeforeVariableItem item in tempItems)
            {
                item.StringRepresentations = Array.Empty<string>();
            }
        }
        
        private void InstantiateItemsInAssembly(Assembly assembly, List<BeforeVariableItem> tempItems, CalculatorOptions options)
        {
            ReadOnlySpan<Type> libraryTypes = assembly.GetTypes();
            foreach (Type type in libraryTypes)
            {
                if (type.IsAbstract) continue;
                if ((options & CalculatorOptions.RemoveDefaultFunctions) == 0 && type.IsSubclassOf(FunctionType) ||
                    (options & CalculatorOptions.RemoveDefaultVariables) == 0 && type.IsSubclassOf(VariableType))
                {
                    BeforeVariableItem instance = (BeforeVariableItem)Activator.CreateInstance(type)!;
                    AddItem(instance, tempItems);
                }
            }
        }
        
        private void InstantiateItemsInAssembly(Assembly assembly, List<BeforeVariableItem> tempItems, CalculatorOptions options, IServiceProvider serviceProvider)
        {
            ReadOnlySpan<Type> libraryTypes = assembly.GetTypes();
            foreach (Type type in libraryTypes)
            {
                if (type.IsAbstract) continue;
                if ((options & CalculatorOptions.RemoveDefaultFunctions) == 0 && type.IsSubclassOf(FunctionType) ||
                    (options & CalculatorOptions.RemoveDefaultVariables) == 0 && type.IsSubclassOf(VariableType))
                {
                    BeforeVariableItem instance = (BeforeVariableItem)ActivatorUtilities.CreateInstance(serviceProvider, type);
                    AddItem(instance, tempItems);
                }
            }
        }
    }
}