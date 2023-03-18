using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Arithmetic
{
    internal sealed class Subtract : OperatorRepresentation
    {
        public override string StringRepresentation => "SUB";
        public override char CharRepresentation => '-';
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Variable Operate(Variable left, Variable right) => left.Double - right.Double;
    }
}