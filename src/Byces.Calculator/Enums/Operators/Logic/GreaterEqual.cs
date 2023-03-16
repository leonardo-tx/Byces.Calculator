using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Logic
{
    internal sealed class GreaterEqual : OperatorRepresentation
    {
        public override string StringRepresentation => ">=";
        internal override OperatorPriority Priority => OperatorPriority.Relational;

        internal override Value Operate(Value left, Value right) => left.Number >= right.Number;
    }
}