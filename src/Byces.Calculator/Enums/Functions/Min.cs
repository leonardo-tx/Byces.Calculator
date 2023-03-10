using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Min : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "MIN";

        public override Value Operate(Value value) => value.Number;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double min = values[0].Number;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].Number < min) min = values[i].Number;
            }
            return min;
        }
    }
}