using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Root : OperatorRepresentation
    {
        public override string StringRepresentation => "RT";
        public override char CharRepresentation => '√';
        internal override OperatorPriority Priority => OperatorPriority.Potentiality;

        internal override Value Operate(Value left, Value right)
        {
            if (left.Number < 0) throw new ArithmeticExpressionException("Attempted to make a negative root.");
            if (right.Number < 0 && left.Number % 2 == 0) throw new ArithmeticExpressionException("Attempted to use an even number to take the root of a negative number.");

            double result;
            if (right.Number < 0)
            {
                result = Math.Pow(right.Number * -1, 1.0 / left.Number) * -1;
            }
            else
            {
                result = Math.Pow(right.Number, 1.0 / left.Number);
            }
            return Math.Round(result, 14);
        }
    }
}