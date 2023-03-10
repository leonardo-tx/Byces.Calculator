using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using MathNet.Numerics;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Factorial : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "FACT";

        public override Value Operate(Value value)
        {
            if (value.Number < 0) throw new ArithmeticExpressionException("Attempted to factorial a negative number.");

            double difference = value.Number - (long)value.Number;
            if (difference == 0) return SpecialFunctions.Factorial((int)value.Number);

            return SpecialFunctions.Gamma(value.Number + 1);
        }
    }
}