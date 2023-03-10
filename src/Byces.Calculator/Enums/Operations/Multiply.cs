using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Multiply : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "MUL";
        public override char CharRepresentation => '*';
        internal override OperatorPriority Priority => OperatorPriority.Second;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Number * secondValue.Number;
    }
}