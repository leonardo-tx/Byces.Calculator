using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Arithmetic
{
    internal sealed class Divide : OperatorRepresentation
    {
        public override string StringRepresentation => "DIV";
        public override char CharRepresentation => '/';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (right.Double == 0) throw new ArithmeticExpressionException("Attempted to divide by zero.");

            return left.Double / right.Double;
        }
    }
}