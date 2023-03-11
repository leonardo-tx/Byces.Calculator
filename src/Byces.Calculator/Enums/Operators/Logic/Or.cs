using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Logic
{
    internal sealed class Or : OperatorRepresentation
    {
        public override string StringRepresentation => "||";
        internal override OperatorPriority Priority => OperatorPriority.Seventh;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            bool boolean1 = firstValue.Boolean;
            bool boolean2 = secondValue.Boolean;

            return boolean1 || boolean2;
        }
    }
}