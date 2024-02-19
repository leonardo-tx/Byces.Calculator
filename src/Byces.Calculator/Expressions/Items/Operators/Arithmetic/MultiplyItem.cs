using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class MultiplyItem : OperatorItem
    {
        public override string StringRepresentation => "MUL";
        public override char CharRepresentation => '*';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Variable Operate(Variable left, Variable right) => left.Double * right.Double;
    }
}