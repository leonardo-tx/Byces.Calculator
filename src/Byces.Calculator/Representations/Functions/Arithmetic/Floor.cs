using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Floor : FunctionRepresentation
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