using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class SineHyperbolicItem : FunctionItem
    {
        public SineHyperbolicItem(): base("SINH")
        {
        }
        
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Sinh(variables[0].Double);
    }
}