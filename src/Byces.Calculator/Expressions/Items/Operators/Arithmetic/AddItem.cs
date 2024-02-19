using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class AddItem : OperatorItem
    {
        public override string StringRepresentation => "ADD";
        public override char CharRepresentation => '+';
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Variable Operate(Variable left, Variable right) => left.Double + right.Double;
    }
}