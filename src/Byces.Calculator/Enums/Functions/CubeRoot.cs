using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CubeRoot : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "CBRT";

        public override Value Operate(Value value) => Math.Cbrt(value.Number);
    }
}