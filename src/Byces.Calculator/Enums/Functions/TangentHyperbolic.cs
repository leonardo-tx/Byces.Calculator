using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class TangentHyperbolic : FunctionType
    {
        protected override int Value => 8;
        protected override string StringRepresentation => "TANH";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Tanh(number);
    }
}