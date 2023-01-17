using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class RootType : OperationType
    {
        internal RootType(int value) : base("Root", value) { }

        internal override string StringRepresentation => "RT";
        internal override char CharRepresentation => '√';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber)
        {
            decimal result = (decimal)Math.Pow(secondNumber, 1 / firstNumber);
            return (double)result;
        }

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}