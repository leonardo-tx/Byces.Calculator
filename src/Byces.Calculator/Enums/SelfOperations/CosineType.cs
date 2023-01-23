using System;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class CosineType : SelfOperationType
    {
        protected override int Value => 3;
        internal override int AdditionalCheck => 1;
        internal override string StringRepresentation => "COS";
        internal override char CharRepresentation => default;

        internal override double Operate(double number)
        {
            double result = Math.Cos(number);
            return Math.Round(result, 15);
        }
    }
}