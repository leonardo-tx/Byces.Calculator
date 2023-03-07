using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Less : OperationType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override char CharRepresentation => '<';
        internal override OperationPriorityType Priority => OperationPriorityType.Fourth;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.ResultType != ResultType.Number || secondValue.ResultType != ResultType.Number) throw new NotSupportedException();

            return firstValue.Number < secondValue.Number;
        }
    }
}