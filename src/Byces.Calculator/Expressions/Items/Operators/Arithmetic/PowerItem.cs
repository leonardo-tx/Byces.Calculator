using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class PowerItem : OperatorItem
    {
        public PowerItem(): base("POW", "**", "^")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.Potentiality;

        internal override Variable Operate(Variable left, Variable right) => Math.Pow(left.Double, right.Double);
    }
}