using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Multiply : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "MUL";
        protected override char CharRepresentation => '*';
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number * secondValue.Number;
    }
}