using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class TangentHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "TANH";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Tanh(variables[0].Double);
    }
}