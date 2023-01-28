using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class Tangent : SelfOperationType
    {
        protected override int Value => 5;
        internal override int AdditionalCheck => 1;
        internal override string StringRepresentation => "TAN";
        internal override char CharRepresentation => default;

        internal override double Operate(double number)
        {
            double degree = number * (180 / Math.PI);
            if (degree == 90 || degree == 270) throw new ArithmeticExpressionException($"Attempted to calculate the tangent of {degree}° in radians.");

            double result = Math.Tan(number);

            return Math.Round(result, 15);
        }
    }
}