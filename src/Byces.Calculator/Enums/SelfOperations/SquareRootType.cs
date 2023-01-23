using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class SquareRootType : SelfOperationType
    {
        protected override int Value => 1;
        internal override string StringRepresentation => "SQRT";
        internal override char CharRepresentation => '√';

        internal override double Operate(double number)
        {
            if (number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(number);
        }
    }
}