using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Degree : FunctionRepresentation
    {
        public override string StringRepresentation => "DEG";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => values[0].Number * 180 / Math.PI;
    }
}