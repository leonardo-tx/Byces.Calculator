using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class And : OperationType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override string StringRepresentation => "&&";
        internal override OperationPriorityType Priority => OperationPriorityType.Sixth;

        internal override Value Operate(Value firstValue, Value secondValue) => firstValue.Boolean && secondValue.Boolean;
    }
}