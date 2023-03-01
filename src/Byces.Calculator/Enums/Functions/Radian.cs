using Byces.Calculator.Exceptions;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Radian : FunctionType
    {
        protected override string StringRepresentation => "RAD";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => number * Constants.Pi2 / 360;

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}