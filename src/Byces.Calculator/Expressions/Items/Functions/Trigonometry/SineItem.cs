using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class SineItem : FunctionItem
    {
        public SineItem(): base("SIN")
        {
        }
        
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double result = Math.Sin(variables[0].Double);
            return Math.Round(result, 15);
        }
    }
}