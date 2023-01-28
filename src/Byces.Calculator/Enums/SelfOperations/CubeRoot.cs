using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class CubeRoot : SelfOperationType
    {
        protected override int Value => 2;
        internal override string StringRepresentation => "CBRT";
        internal override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Cbrt(number);
    }
}