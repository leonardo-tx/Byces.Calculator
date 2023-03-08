using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal class Or : OperationType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override string StringRepresentation => "||";
        internal override OperationPriorityType Priority => OperationPriorityType.Seventh;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            bool boolean1 = firstValue.Boolean;
            bool boolean2 = secondValue.Boolean;

            return boolean1 || boolean2;
        }
    }
}