using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Root : OperatorRepresentation
    {
        public override string StringRepresentation => "RT";
        public override char CharRepresentation => '√';
        internal override OperatorPriority Priority => OperatorPriority.First;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.Number < 0) throw new ArithmeticExpressionException("Attempted to make a negative root.");
            if (secondValue.Number < 0 && firstValue.Number % 2 == 0) throw new ArithmeticExpressionException("Attempted to use an even number to take the root of a negative number.");

            double result;
            if (secondValue.Number < 0)
            {
                result = Math.Pow(secondValue.Number * -1, 1.0 / firstValue.Number) * -1;
            }
            else
            {
                result = Math.Pow(secondValue.Number, 1.0 / firstValue.Number);
            }
            return Math.Round(result, 14);
        }
    }
}