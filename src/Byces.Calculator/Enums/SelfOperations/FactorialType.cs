using Byces.Calculator.Exceptions;
using MathNet.Numerics;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class FactorialType : SelfOperationType
    {
        protected override int Value => 0;
        internal override string StringRepresentation => "FACT";
        internal override char CharRepresentation => default;

        internal override double Operate(double number)
        {
            if (number < 0) throw new ArithmeticExpressionException("Attempted to factorial a negative number.");

            double difference = number - (long)number;
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }
    }
}