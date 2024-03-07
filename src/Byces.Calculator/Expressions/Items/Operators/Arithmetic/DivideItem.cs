using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Arithmetic
{
    internal sealed class DivideItem : OperatorItem
    {
        public DivideItem(): base("DIV", "/")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Variable Operate(Variable left, Variable right) => left.Double / right.Double;
    }
}