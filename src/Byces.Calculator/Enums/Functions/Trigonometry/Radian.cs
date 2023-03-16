using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Radian : FunctionRepresentation
    {
        public override string StringRepresentation => "RAD";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => values[0].Number * Math.PI / 180;
    }
}