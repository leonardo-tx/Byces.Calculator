using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Arithmetic
{
    internal sealed class Multiply : OperatorRepresentation
    {
        public override string StringRepresentation => "MUL";
        public override char CharRepresentation => '*';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Variable Operate(Variable left, Variable right) => left.Double * right.Double;
    }
}