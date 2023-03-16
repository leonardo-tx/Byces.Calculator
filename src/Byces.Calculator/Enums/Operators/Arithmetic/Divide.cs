using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Divide : OperatorRepresentation
    {
        public override string StringRepresentation => "DIV";
        public override char CharRepresentation => '/';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Value Operate(Value left, Value right)
        {
            if (right.Number == 0) throw new ArithmeticExpressionException("Attempted to divide by zero.");

            return left.Number / right.Number;
        }
    }
}