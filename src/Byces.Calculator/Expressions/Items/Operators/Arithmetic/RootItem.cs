using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class RootItem : OperatorItem
    {
        public override string StringRepresentation => "RT";
        public override char CharRepresentation => '√';
        internal override OperatorPriority Priority => OperatorPriority.Potentiality;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (right.Double < 0 && left.Double % 2 == 0) return double.NaN;

            double result;
            if (right.Double < 0)
            {
                result = Math.Pow(right.Double * -1, 1.0 / left.Double) * -1;
            }
            else
            {
                result = Math.Pow(right.Double, 1.0 / left.Double);
            }
            return Math.Round(result, 14);
        }
    }
}