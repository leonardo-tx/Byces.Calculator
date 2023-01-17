using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class MultiplyType : OperationType
    {
        internal MultiplyType(int value) : base("Multiply", value) { }

        internal override string StringRepresentation => "MUL";
        internal override char CharRepresentation => '*';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber * secondNumber;

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}