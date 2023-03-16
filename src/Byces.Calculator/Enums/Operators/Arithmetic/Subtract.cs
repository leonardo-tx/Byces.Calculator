using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Subtract : OperatorRepresentation
    {
        public override string StringRepresentation => "SUB";
        public override char CharRepresentation => '-';
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Value Operate(Value left, Value right) => left.Number - right.Number;
    }
}