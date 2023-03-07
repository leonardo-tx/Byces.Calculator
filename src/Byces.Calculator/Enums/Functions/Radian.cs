using Byces.Calculator.Expressions;
using MathNet.Numerics;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Radian : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "RAD";

        public override Value Operate(Value value) => value.Number * Constants.Pi2 / 360;
    }
}