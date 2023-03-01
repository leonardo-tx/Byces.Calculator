using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CubeRoot : FunctionType
    {
        protected override string StringRepresentation => "CBRT";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => Math.Cbrt(number);

        public override double Operate(ReadOnlySpan<double> numbers) => throw new UnsupportedFunctionExpressionException();
    }
}