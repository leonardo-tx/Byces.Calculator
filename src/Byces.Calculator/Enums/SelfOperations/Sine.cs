using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class Sine : SelfOperationType
    {
        protected override int Value => 4;
        internal override int AdditionalCheck => 1;
        internal override string StringRepresentation => "SIN";
        internal override char CharRepresentation => default;

        internal override double Operate(double number)
        {
            double result = Math.Sin(number);
            return Math.Round(result, 15);
        }
    }
}