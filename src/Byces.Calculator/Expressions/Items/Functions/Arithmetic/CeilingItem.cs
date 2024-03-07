using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class CeilingItem : FunctionItem
    {
        public CeilingItem(): base("CEIL")
        {
        }

        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return Math.Ceiling(variables[0].Double);
        }
    }
}