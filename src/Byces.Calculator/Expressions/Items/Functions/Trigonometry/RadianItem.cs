using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class RadianItem : FunctionItem
    {
        public override string StringRepresentation => "RAD";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => variables[0].Double * Math.PI / 180;
    }
}