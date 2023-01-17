using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class SineType : OperationType
    {
        internal SineType(int value) : base("Sine", value) { }

        internal override string StringRepresentation => "SIN";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Sin(number);
    }
}