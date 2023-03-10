using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums
{
    internal abstract class FunctionRepresentation : ExpressionRepresentation<FunctionRepresentation>
    {
        public static implicit operator int(FunctionRepresentation representation) => representation.Value;
        public static explicit operator FunctionRepresentation(int value) => GetItem(value);

        protected FunctionRepresentation() : base(ExpressionConflict.Function | ExpressionConflict.Variable) { }

        public virtual Value Operate(Value value) => throw new UnsupportedFunctionExpressionException();

        public virtual Value Operate(ReadOnlySpan<Value> values) => throw new UnsupportedFunctionExpressionException();

        internal override ExpressionConflict RepresentationConflict => ExpressionConflict.Function;
    }
}