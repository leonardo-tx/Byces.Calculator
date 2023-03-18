using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations
{
    internal abstract class OperatorRepresentation : ExpressionRepresentation<OperatorRepresentation>
    {
        public static implicit operator int(OperatorRepresentation representation) => representation.Value;
        public static explicit operator OperatorRepresentation(int value) => GetItem(value);

        protected OperatorRepresentation() : base(ExpressionConflict.Operator, ExpressionConflict.Operator) { }

        internal abstract OperatorPriority Priority { get; }

        internal abstract Variable Operate(Variable left, Variable right);
    }
}