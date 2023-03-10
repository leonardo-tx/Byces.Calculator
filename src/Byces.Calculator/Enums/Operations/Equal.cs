using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Equal : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override string StringRepresentation => "==";
        internal override OperatorPriority Priority => OperatorPriority.Fifth;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.ResultType != secondValue.ResultType) return false;

            return firstValue.ResultType switch
            {
                ResultType.Boolean => firstValue.Boolean == secondValue.Boolean,
                ResultType.Number => firstValue.Number == secondValue.Number,
                _ => throw new NotSupportedException(),
            };
        }
    }
}