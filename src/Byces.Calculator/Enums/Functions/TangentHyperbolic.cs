using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class TangentHyperbolic : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "TANH";

        public override Value Operate(Value value) => Math.Tanh(value.Number);
    }
}