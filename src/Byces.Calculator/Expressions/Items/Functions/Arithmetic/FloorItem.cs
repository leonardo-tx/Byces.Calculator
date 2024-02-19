using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class FloorItem : FunctionItem
    {
        public override string StringRepresentation => "FLOOR";

        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return Math.Floor(variables[0].Double);
        }
    }
}