using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SquareRoot : FunctionType
    {
        protected override string StringRepresentation => "SQRT";
        protected override char CharRepresentation => '√';

        public override double Operate(double number)
        {
            if (number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(number);
        }

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}