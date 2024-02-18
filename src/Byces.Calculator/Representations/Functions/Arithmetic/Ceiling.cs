using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Ceiling : FunctionRepresentation
    {
        public override string StringRepresentation => "CEIL";

        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return Math.Ceiling(variables[0].Double);
        }
    }
}