using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Power : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "POW";
        public override char CharRepresentation => '^';
        internal override OperatorPriority Priority => OperatorPriority.First;

        internal override Value Operate(Value firstValue, Value secondValue) => Math.Pow(firstValue.Number, secondValue.Number);
    }
}