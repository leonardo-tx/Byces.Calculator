using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class Logarithm : SelfOperationType
    {
        protected override int Value => 10;
        internal override string StringRepresentation => "LOG";
        internal override char CharRepresentation => default;

        internal override double Operate(double number)
        {
            if (number <= 0) throw new ArithmeticExpressionException($"Attempted to log (base 10) of {number}");
            return Math.Log10(number);
        }
    }
}