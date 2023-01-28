using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class CosineHyperbolic : SelfOperationType
    {
        protected override int Value => 6;
        internal override string StringRepresentation => "COSH";
        internal override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Cosh(number);
    }
}