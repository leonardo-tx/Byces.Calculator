using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums
{
    internal abstract class OperatorRepresentation : ExpressionRepresentation<OperatorRepresentation>
    {
        public static implicit operator int(OperatorRepresentation representation) => representation.Value;
        public static explicit operator OperatorRepresentation(int value) => GetItem(value);

        protected OperatorRepresentation() : base(ExpressionConflict.Operator) { }

        internal abstract OperatorPriority Priority { get; }

        internal virtual Value Operate(Value firstValue, Value secondValue) => throw new NotSupportedException();

        internal override ExpressionConflict RepresentationConflict => ExpressionConflict.Operator;
    }
}