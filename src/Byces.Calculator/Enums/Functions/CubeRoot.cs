using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CubeRoot : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "CBRT";

        public override Value Operate(Value value) => Math.Cbrt(value.Number);
    }
}