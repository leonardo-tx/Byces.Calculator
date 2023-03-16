using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Arithmetic
{
    internal sealed class CubeRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "CBRT";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => Math.Cbrt(values[0].Number);
    }
}