using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Degree : FunctionRepresentation
    {
        public override string StringRepresentation => "DEG";

        public override Value Operate(Value value) => value.Number * 180 / Math.PI;
    }
}