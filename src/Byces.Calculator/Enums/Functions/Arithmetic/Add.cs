using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Arithmetic
{
    internal sealed class Add : FunctionRepresentation
    {
        public override string StringRepresentation => "ADD";

        public override Value Operate(Value value) => value.Number;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double result = 0;
            for (int i = 0; i < values.Length; i++)
            {
                result += values[i].Number;
            }
            return result;
        }
    }
}