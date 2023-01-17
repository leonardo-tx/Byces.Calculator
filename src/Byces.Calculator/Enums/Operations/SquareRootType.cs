using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class SquareRootType : OperationType
    {
        internal SquareRootType(int value) : base("SquareRoot", value) { }

        internal override string StringRepresentation => "SQRT";
        internal override char CharRepresentation => '√';
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Sqrt(number);
    }
}