﻿using MathNet.Numerics;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class FactorialType : OperationType
    {
        internal FactorialType(int value) : base("Factorial", value) { }

        internal override string StringRepresentation => string.Empty;
        internal override char CharRepresentation => '!';
        internal override OperationCategory Category => OperationCategory.After;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number)
        {
            if (number < 0) return double.NaN;

            double difference = number - (int)number;
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }
    }
}