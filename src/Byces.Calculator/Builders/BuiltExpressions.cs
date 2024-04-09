using System;
using System.Reflection;
using Byces.Calculator.Enums;

namespace Byces.Calculator.Builders
{
    internal sealed class BuiltExpressions
    {
        public BuiltExpressions(CalculatorOptions options, Assembly[] assemblies, IServiceProvider? serviceProvider)
        {
            Assembly libraryAssembly = typeof(BuiltExpressions).Assembly;
            BeforeConflictItems = new BeforeConflictItems(libraryAssembly, assemblies, serviceProvider, options);
            AfterConflictItems = new AfterConflictItems(libraryAssembly);
        }

        internal readonly BeforeConflictItems BeforeConflictItems;
        internal readonly AfterConflictItems AfterConflictItems;
    }
}