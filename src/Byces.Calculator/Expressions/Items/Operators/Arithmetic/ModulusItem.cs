using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class ModulusItem : OperatorItem
    {
        public override string StringRepresentation => "MOD";
        public override char CharRepresentation => '%';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (right.Double == 0) return left.Double;
            if (left.Double < 0 && right.Double > 0)
            {
                double result = right.Double - left.Double * -1 % right.Double;
                if (result == right.Double) return 0;

                return result;
            }
            if (left.Double > 0 && right.Double < 0)
            {
                double result = right.Double - left.Double % right.Double * -1;
                if (result == right.Double) return 0;

                return result;
            }
            return left.Double % right.Double;
        }
    }
}