using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Operators.Special
{
    internal sealed class SemiColon : OperatorRepresentation
    {
        public override char CharRepresentation => ';';
        internal override OperatorPriority Priority => OperatorPriority.SemiColon;

        internal override Variable Operate(Variable left, Variable right) => throw new NotSupportedException();
    }
}