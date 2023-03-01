using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Logarithm : FunctionType
    {
        protected override string StringRepresentation => "LOG";
        protected override char CharRepresentation => default;

        public override double Operate(double number)
        {
            if (number <= 0) throw new ArithmeticExpressionException($"Attempted to log (base 10) of {number}");
            return Math.Log10(number);
        }

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}