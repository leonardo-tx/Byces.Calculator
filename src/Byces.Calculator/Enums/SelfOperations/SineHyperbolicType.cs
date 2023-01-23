using System;
using Byces.Calculator.Enums.Operations;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class SineHyperbolicType : SelfOperationType
    {
        protected override int Value => 7;
        internal override string StringRepresentation => "SINH";
        internal override char CharRepresentation => default;

        internal override double Operate(double number) => Math.Sinh(number);
    }
}