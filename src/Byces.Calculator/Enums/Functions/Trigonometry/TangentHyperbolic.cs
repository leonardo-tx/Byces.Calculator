using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class TangentHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "TANH";

        public override Value Operate(Value value) => Math.Tanh(value.Number);
    }
}