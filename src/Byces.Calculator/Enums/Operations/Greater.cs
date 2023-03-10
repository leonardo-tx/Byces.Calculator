﻿using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Greater : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override char CharRepresentation => '>';
        internal override OperatorPriority Priority => OperatorPriority.Fourth;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.ResultType != ResultType.Number || secondValue.ResultType != ResultType.Number) throw new NotSupportedException();

            return firstValue.Number > secondValue.Number;
        }
    }
}