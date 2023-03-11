using System;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class SineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "SINH";

        public override Value Operate(Value value) => Math.Sinh(value.Number);
    }
}