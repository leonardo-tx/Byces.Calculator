using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Cosine : FunctionRepresentation
    {
        public override string StringRepresentation => "COS";

        public override Value Operate(Value value)
        {
            double result = Math.Cos(value.Number);
            return Math.Round(result, 15);
        }
    }
}