using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums
{
    internal abstract class FunctionRepresentation : ExpressionRepresentation<FunctionRepresentation>
    {
        public static implicit operator int(FunctionRepresentation representation) => representation.Value;
        public static explicit operator FunctionRepresentation(int value) => GetItem(value);

        protected FunctionRepresentation() : base(ExpressionConflict.Function | ExpressionConflict.Variable, ExpressionConflict.Function) { }

        public virtual int ParametersMin => 1;

        public virtual int ParametersMax => -1;

        public virtual Value Operate(ReadOnlySpan<Value> values) => throw new UnsupportedFunctionExpressionException();
    }
}