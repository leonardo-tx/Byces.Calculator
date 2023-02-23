using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SquareRoot : FunctionType
    {
        protected override int Value => 1;
        protected override string StringRepresentation => "SQRT";
        protected override char CharRepresentation => '√';

        internal override double Operate(double number)
        {
            if (number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(number);
        }
    }
}