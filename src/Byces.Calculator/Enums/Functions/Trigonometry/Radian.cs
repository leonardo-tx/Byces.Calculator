using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Trigonometry
{
    internal sealed class Radian : FunctionRepresentation
    {
        public override string StringRepresentation => "RAD";

        public override Value Operate(Value value) => value.Number * Math.PI / 180;
    }
}