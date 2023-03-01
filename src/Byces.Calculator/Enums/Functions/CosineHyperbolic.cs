using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CosineHyperbolic : FunctionType
    {
        protected override string StringRepresentation => "COSH";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => Math.Cosh(number);

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}