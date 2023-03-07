using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums
{
    internal abstract class FunctionType : ExpressionType<FunctionType>
    {
        public static implicit operator int(FunctionType functionType) => functionType.Value;
        public static explicit operator FunctionType(int value) => GetItem(value);

        public virtual Value Operate(Value value) => throw new UnsupportedFunctionExpressionException();

        public virtual Value Operate(ReadOnlySpan<Value> values) => throw new UnsupportedFunctionExpressionException();
    }
}