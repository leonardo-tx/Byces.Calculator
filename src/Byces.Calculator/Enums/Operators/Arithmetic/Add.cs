using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Add : OperatorRepresentation
    {
        public override string StringRepresentation => "ADD";
        public override char CharRepresentation => '+';
        internal override OperatorPriority Priority => OperatorPriority.Third;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number + secondValue.Number;
    }
}