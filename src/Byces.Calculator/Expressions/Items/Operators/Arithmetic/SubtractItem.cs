using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class SubtractItem : OperatorItem
    {
        public SubtractItem(): base("SUB", "-")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.Additive;

        internal override Variable Operate(Variable left, Variable right) => left.Double - right.Double;
    }
}