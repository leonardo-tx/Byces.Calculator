using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class MinItem : FunctionItem
    {
        public override string StringRepresentation => "MIN";
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double min = variables[0].Double;
            for (int i = 1; i < variables.Length; i++)
            {
                if (variables[i].Double < min) min = variables[i].Double;
            }
            return min;
        }
    }
}