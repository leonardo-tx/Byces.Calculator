using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class PowerType : OperationType
    {
        internal PowerType(int value) : base("Power", value) { }

        internal override string StringRepresentation => "POW";
        internal override char CharRepresentation => '^';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => Math.Pow(firstNumber, secondNumber);

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}