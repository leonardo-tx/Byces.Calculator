using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Subtract : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "SUB";
        protected override char CharRepresentation => '-';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number - secondValue.Number;
    }
}