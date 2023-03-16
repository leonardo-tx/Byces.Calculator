using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Enums.Functions.Arithmetic
{
    internal sealed class Factorial : FunctionRepresentation
    {
        public override string StringRepresentation => "FACT";
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            double number = values[0].Number;
            if (number < 0) throw new ArithmeticExpressionException("Attempted to factorial a negative number.");

            double difference = number - (long)number;
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }
    }
}