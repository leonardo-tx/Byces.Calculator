using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Subtract : OperatorRepresentation
    {
        public override string StringRepresentation => "SUB";
        public override char CharRepresentation => '-';
        internal override OperatorPriority Priority => OperatorPriority.Third;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number - secondValue.Number;
    }
}