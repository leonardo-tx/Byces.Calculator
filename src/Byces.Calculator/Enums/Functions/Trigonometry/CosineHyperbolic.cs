using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class CosineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "COSH";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => Math.Cosh(values[0].Number);
    }
}