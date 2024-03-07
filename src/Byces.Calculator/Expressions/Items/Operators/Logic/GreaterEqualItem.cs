using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class GreaterEqualItem : OperatorItem
    {
        public GreaterEqualItem(): base(">=")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.Relational;

        internal override Variable Operate(Variable left, Variable right) => left.Double >= right.Double;
    }
}