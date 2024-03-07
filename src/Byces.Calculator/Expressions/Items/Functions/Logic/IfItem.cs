using System;

namespace Byces.Calculator.Expressions.Items.Functions.Logic
{
    internal sealed class IfItem : FunctionItem
    {
        public IfItem(): base("IF")
        {
        }

        public override int ParametersMin => 3;
        public override int ParametersMax => 3;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return variables[0].Bool ? variables[1] : variables[2];
        }
    }
}