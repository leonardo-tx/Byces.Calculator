using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Arithmetic
{
    internal sealed class SquareRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "SQRT";
        public override char CharRepresentation => '√';

        public override Value Operate(Value value)
        {
            if (value.Number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(value.Number);
        }
    }
}