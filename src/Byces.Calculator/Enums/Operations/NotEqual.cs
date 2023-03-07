using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class NotEqual : OperationType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override string StringRepresentation => "!=";
        internal override OperationPriorityType Priority => OperationPriorityType.Fifth;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.ResultType != secondValue.ResultType) return true;

            return firstValue.ResultType switch
            {
                ResultType.Boolean => firstValue.Boolean != secondValue.Boolean,
                ResultType.Number => firstValue.Number != secondValue.Number,
                _ => throw new NotSupportedException(),
            };
        }
    }
}