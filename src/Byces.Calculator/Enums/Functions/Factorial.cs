using Byces.Calculator.Exceptions;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Factorial : FunctionType
    {
        protected override string StringRepresentation => "FACT";
        protected override char CharRepresentation => default;

        public override double Operate(double number)
        {
            if (number < 0) throw new ArithmeticExpressionException("Attempted to factorial a negative number.");

            double difference = number - (long)number;
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}