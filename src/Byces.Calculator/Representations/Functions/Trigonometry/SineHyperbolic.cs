using System;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class SineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "SINH";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Sinh(variables[0].Double);
    }
}