using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class GreaterItem : OperatorItem
    {
        public override char CharRepresentation => '>';
        internal override OperatorPriority Priority => OperatorPriority.Relational;

        internal override Variable Operate(Variable left, Variable right) => left.Double > right.Double;
    }
}