using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class CosineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "COSH";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Cosh(variables[0].Double);
    }
}