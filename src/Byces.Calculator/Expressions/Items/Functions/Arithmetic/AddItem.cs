using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class AddItem : FunctionItem
    {
        public AddItem(): base("ADD")
        {
        }
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double result = 0;
            for (int i = 0; i < variables.Length; i++)
            {
                result += variables[i].Double;
            }
            return result;
        }
    }
}