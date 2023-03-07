using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CosineHyperbolic : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "COSH";

        public override Value Operate(Value value) => Math.Cosh(value.Number);
    }
}