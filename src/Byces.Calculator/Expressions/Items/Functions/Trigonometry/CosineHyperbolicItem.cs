using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class CosineHyperbolicItem : FunctionItem
    {
        public override string StringRepresentation => "COSH";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Cosh(variables[0].Double);
    }
}