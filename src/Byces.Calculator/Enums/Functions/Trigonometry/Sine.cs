using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Sine : FunctionRepresentation
    {
        public override string StringRepresentation => "SIN";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double result = Math.Sin(values[0].Number);
            return Math.Round(result, 15);
        }
    }
}