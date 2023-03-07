using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Add : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "ADD";
        protected override char CharRepresentation => '+';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number + secondValue.Number;
    }
}