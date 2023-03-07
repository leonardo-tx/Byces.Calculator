using System;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SineHyperbolic : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "SINH";

        public override Value Operate(Value value) => Math.Sinh(value.Number);
    }
}