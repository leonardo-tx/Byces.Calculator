using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class Tangent : FunctionRepresentation
    {
        public override string StringRepresentation => "TAN";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;
            double degree = number * 180 / Math.PI;
            if (degree % 360.00 == 90.00) throw new ArithmeticExpressionException($"Attempted to calculate the tangent of {degree}° in radians.");

            double result = Math.Tan(number);
            return Math.Round(result, 15);
        }
    }
}