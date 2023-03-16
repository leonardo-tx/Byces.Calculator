﻿using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Multiply : OperatorRepresentation
    {
        public override string StringRepresentation => "MUL";
        public override char CharRepresentation => '*';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Value Operate(Value left, Value right) => left.Number * right.Number;
    }
}