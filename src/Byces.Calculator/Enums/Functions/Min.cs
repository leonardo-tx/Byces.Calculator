using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Min : FunctionType
    {
        protected override string StringRepresentation => "MIN";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => number;

        public override double Operate(ReadOnlySpan<double> numbers)
        {
            double min = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < min) min = numbers[i];
            }
            return min;
        }
    }
}