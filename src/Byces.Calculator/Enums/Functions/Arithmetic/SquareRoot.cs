using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Arithmetic
{
    internal sealed class SquareRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "SQRT";
        public override char CharRepresentation => '√';
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double number = values[0].Number;

            if (number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(number);
        }
    }
}