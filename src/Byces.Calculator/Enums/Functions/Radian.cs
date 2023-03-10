using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Radian : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "RAD";

        public override Value Operate(Value value) => value.Number * Math.PI / 180;
    }
}