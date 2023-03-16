using System;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class SineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "SINH";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => Math.Sinh(values[0].Number);
    }
}