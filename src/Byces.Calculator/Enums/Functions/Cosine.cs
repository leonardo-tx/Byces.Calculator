using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Cosine : FunctionType
    {
        protected override string StringRepresentation => "COS";
        protected override char CharRepresentation => default;
        internal override int AdditionalCheck => 1;

        public override double Operate(double number)
        {
            double result = Math.Cos(number);
            return Math.Round(result, 15);
        }

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}