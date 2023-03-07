using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SquareRoot : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "SQRT";
        protected override char CharRepresentation => '√';

        public override Value Operate(Value value)
        {
            if (value.Number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(value.Number);
        }
    }
}