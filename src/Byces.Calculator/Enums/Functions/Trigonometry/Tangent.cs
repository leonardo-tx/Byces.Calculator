using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Tangent : FunctionRepresentation
    {
        public override string StringRepresentation => "TAN";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double number = values[0].Number;
            double degree = number * 180 / Math.PI;
            if (degree == 90 || degree == 270) throw new ArithmeticExpressionException($"Attempted to calculate the tangent of {degree}° in radians.");

            double result = Math.Tan(number);
            return Math.Round(result, 15);
        }
    }
}