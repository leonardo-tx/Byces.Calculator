using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class AndItem : OperatorItem
    {
        public AndItem(): base("&&")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.AndConditional;

        internal override Variable Operate(Variable left, Variable right)
        {
            bool boolean1 = left.Bool;
            bool boolean2 = right.Bool;

            return boolean1 && boolean2;
        }
    }
}