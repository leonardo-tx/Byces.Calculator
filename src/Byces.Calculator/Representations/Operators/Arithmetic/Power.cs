using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Operators.Arithmetic
{
    internal sealed class Power : OperatorRepresentation
    {
        public override string StringRepresentation => "POW";
        public override char CharRepresentation => '^';
        internal override OperatorPriority Priority => OperatorPriority.Potentiality;

        internal override Variable Operate(Variable left, Variable right) => Math.Pow(left.Double, right.Double);
    }
}