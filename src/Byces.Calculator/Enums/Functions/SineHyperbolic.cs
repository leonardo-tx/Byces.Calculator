using System;
using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class SineHyperbolic : FunctionType
    {
        protected override string StringRepresentation => "SINH";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => Math.Sinh(number);

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}