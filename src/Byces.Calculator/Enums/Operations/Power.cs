using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Power : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "POW";
        protected override char CharRepresentation => '^';
        internal override OperationPriorityType Priority => OperationPriorityType.First;

        internal override Value Operate(Value firstValue, Value secondValue) => Math.Pow(firstValue.Number, secondValue.Number);
    }
}