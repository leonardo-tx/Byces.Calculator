using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class TangentHyperbolic : FunctionType
    {
        protected override string StringRepresentation => "TANH";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => Math.Tanh(number);

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}