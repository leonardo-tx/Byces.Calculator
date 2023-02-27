using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Cosine : FunctionType
    {
        protected override int Value => 3;
        protected override string StringRepresentation => "COS";
        protected override char CharRepresentation => default;
        internal override int AdditionalCheck => 1;

        internal override double Operate(double number)
        {
            double result = Math.Cos(number);
            return Math.Round(result, 15);
        }

        internal override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}