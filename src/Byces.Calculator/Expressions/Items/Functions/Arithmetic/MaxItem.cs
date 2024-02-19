using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class MaxItem : FunctionItem
    {
        public override string StringRepresentation => "MAX";
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double max = variables[0].Double;
            for (int i = 1; i < variables.Length; i++)
            {
                if (variables[i].Double > max) max = variables[i].Double;
            }
            return max;
        }
    }
}