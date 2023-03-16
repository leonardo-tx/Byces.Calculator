using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class TangentHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "TANH";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => Math.Tanh(values[0].Number);
    }
}