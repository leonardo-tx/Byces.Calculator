using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Add : FunctionType
    {
        protected override int Value => 11;
        protected override string StringRepresentation => "ADD";
        protected override char CharRepresentation => default;

        internal override double Operate(double number) => number;

        internal override double Operate(ReadOnlySpan<double> numbers)
        {
            double result = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                result += numbers[i];
            }
            return result;
        }
    }
}