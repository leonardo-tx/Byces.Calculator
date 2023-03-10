using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Tangent : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "TAN";

        public override Value Operate(Value value)
        {
            double degree = value.Number * (180 / Math.PI);
            if (degree == 90 || degree == 270) throw new ArithmeticExpressionException($"Attempted to calculate the tangent of {degree}° in radians.");

            double result = Math.Tan(value.Number);

            return Math.Round(result, 15);
        }
    }
}