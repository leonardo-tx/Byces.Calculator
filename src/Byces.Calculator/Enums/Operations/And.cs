﻿using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class And : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override string StringRepresentation => "&&";
        internal override OperatorPriority Priority => OperatorPriority.Sixth;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            bool boolean1 = firstValue.Boolean;
            bool boolean2 = secondValue.Boolean;

            return boolean1 && boolean2;
        }
    }
}