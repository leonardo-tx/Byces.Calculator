using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Max : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "MAX";

        public override Value Operate(Value value) => value.Number;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double max = values[0].Number;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].Number > max) max = values[i].Number;
            }
            return max;
        }
    }
}