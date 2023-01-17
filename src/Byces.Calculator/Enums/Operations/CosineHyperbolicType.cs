using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class CosineHyperbolicType : OperationType
    {
        internal CosineHyperbolicType(int value) : base("CosineHyperbolic", value) { }

        internal override string StringRepresentation => "COSH";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Cosh(number);
    }
}