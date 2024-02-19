using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Special
{
    internal sealed class SemiColonItem : OperatorItem
    {
        public override char CharRepresentation => ';';
        internal override OperatorPriority Priority => OperatorPriority.SemiColon;

        internal override Variable Operate(Variable left, Variable right) => throw new NotSupportedException();
    }
}