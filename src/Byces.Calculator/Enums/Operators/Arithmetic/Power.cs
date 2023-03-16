using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Power : OperatorRepresentation
    {
        public override string StringRepresentation => "POW";
        public override char CharRepresentation => '^';
        internal override OperatorPriority Priority => OperatorPriority.Potentiality;

        internal override Value Operate(Value left, Value right) => Math.Pow(left.Number, right.Number);
    }
}