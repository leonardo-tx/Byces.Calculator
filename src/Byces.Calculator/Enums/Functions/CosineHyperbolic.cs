using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CosineHyperbolic : FunctionType
    {
        protected override int Value => 6;
        protected override string StringRepresentation => "COSH";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Cosh(number);

        internal override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}