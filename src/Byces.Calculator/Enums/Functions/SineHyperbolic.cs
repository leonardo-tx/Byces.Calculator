using System;
using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SineHyperbolic : FunctionType
    {
        protected override int Value => 7;
        protected override string StringRepresentation => "SINH";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Sinh(number);

        internal override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}