using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Tangent : FunctionType
    {
        protected override string StringRepresentation => "TAN";
        protected override char CharRepresentation => default;
        internal override int AdditionalCheck => 1;

        public override double Operate(double number)
        {
            double degree = number * (180 / Math.PI);
            if (degree == 90 || degree == 270) throw new ArithmeticExpressionException($"Attempted to calculate the tangent of {degree}° in radians.");

            double result = Math.Tan(number);

            return Math.Round(result, 15);
        }

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}