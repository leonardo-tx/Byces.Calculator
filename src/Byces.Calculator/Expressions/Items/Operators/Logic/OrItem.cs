using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class OrItem : OperatorItem
    {
        public override string StringRepresentation => "||";
        internal override OperatorPriority Priority => OperatorPriority.OrConditional;

        internal override Variable Operate(Variable left, Variable right)
        {
            bool boolean1 = left.Bool;
            bool boolean2 = right.Bool;

            return boolean1 || boolean2;
        }
    }
}