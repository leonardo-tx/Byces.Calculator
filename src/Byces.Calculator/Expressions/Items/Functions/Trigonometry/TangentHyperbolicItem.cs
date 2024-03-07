using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class TangentHyperbolicItem : FunctionItem
    {
        public TangentHyperbolicItem(): base("TANH")
        {
        }
        
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Tanh(variables[0].Double);
    }
}