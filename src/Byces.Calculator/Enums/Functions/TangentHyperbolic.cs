using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class TangentHyperbolic : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "TANH";

        public override Value Operate(Value value) => Math.Tanh(value.Number);
    }
}