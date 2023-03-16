using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Add : OperatorRepresentation
    {
        public override string StringRepresentation => "ADD";
        public override char CharRepresentation => '+';
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Value Operate(Value left, Value right) => left.Number + right.Number;
    }
}