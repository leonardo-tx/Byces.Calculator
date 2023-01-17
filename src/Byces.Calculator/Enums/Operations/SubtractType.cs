using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class SubtractType : OperationType
    {
        internal SubtractType(int value) : base("Subtract", value) { }

        internal override string StringRepresentation => "SUB";
        internal override char CharRepresentation => '-';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber - secondNumber;

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}