using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Cosine : FunctionRepresentation
    {
        public override string StringRepresentation => "COS";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double result = Math.Cos(values[0].Number);
            return Math.Round(result, 15);
        }
    }
}