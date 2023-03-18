using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Logic
{
    internal sealed class Or : OperatorRepresentation
    {
        public override string StringRepresentation => "||";
        internal override OperatorPriority Priority => OperatorPriority.OrConditional;

        internal override Variable Operate(Variable left, Variable right)
        {
            bool boolean1 = left.Boolean;
            bool boolean2 = right.Boolean;

            return boolean1 || boolean2;
        }
    }
}