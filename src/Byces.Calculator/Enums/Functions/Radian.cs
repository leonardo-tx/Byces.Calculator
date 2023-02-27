using Byces.Calculator.Exceptions;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Radian : FunctionType
    {
        protected override int Value => 9;
        protected override string StringRepresentation => "RAD";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => number * Constants.Pi2 / 360;

        internal override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}