using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class DivideType : OperationType
    {
        internal DivideType(int value) : base("Divide", value) { }

        internal override string StringRepresentation => "DIV";
        internal override char CharRepresentation => '/';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber / secondNumber;

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}