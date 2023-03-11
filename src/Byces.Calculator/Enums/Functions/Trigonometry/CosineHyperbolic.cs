using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class CosineHyperbolic : FunctionRepresentation
    {
        public override string StringRepresentation => "COSH";

        public override Value Operate(Value value) => Math.Cosh(value.Number);
    }
}