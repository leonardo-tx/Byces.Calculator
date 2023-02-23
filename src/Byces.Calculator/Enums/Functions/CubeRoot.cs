using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class CubeRoot : FunctionType
    {
        protected override int Value => 2;
        protected override string StringRepresentation => "CBRT";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Cbrt(number);
    }
}