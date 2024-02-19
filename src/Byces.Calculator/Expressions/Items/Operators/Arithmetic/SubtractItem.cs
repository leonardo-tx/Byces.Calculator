using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class SubtractItem : OperatorItem
    {
        public override string StringRepresentation => "SUB";
        public override char CharRepresentation => '-';
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Variable Operate(Variable left, Variable right) => left.Double - right.Double;
    }
}