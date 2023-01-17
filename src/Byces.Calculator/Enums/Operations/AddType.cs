using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class AddType : OperationType
    {
        internal AddType(int value) : base("Add", value) { }

        internal override string StringRepresentation => "ADD";
        internal override char CharRepresentation => '+';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber + secondNumber;

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}