using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Max : FunctionType
    {
        protected override string StringRepresentation => "MAX";
        protected override char CharRepresentation => default;

        public override double Operate(double number) => number;

        public override double Operate(ReadOnlySpan<double> numbers)
        {
            double max = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > max) max = numbers[i];
            }
            return max;
        }
    }
}