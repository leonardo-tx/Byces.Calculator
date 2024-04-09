using System;
using System.Collections.Generic;
using System.Reflection;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Builders
{
    internal sealed class AfterConflictItems : ConflictItems<OperatorItem>
    {
        private static readonly Type OperatorType = typeof(OperatorItem);
        
        public AfterConflictItems(Assembly libraryAssembly)
        {
            List<OperatorItem> tempItems = new();
            InstantiateItemsInAssembly(libraryAssembly, tempItems);
            
            foreach (OperatorItem item in tempItems)
            {
                item.StringRepresentations = Array.Empty<string>();
            }
        }
        
        private void InstantiateItemsInAssembly(Assembly assembly, List<OperatorItem> tempItems)
        {
            ReadOnlySpan<Type> libraryTypes = assembly.GetTypes();
            foreach (Type type in libraryTypes)
            {
                if (type.IsAbstract || !type.IsSubclassOf(OperatorType)) continue;

                OperatorItem instance = (OperatorItem)Activator.CreateInstance(type)!;
                AddItem(instance, tempItems);
            }
        }
    }
}