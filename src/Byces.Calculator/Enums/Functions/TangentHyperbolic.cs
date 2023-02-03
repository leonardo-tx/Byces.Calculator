using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class TangentHyperbolic : FunctionType
    {
        protected override int Value => 8;
        internal override string StringRepresentation => "TANH";
        internal override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Tanh(number);
    }
}