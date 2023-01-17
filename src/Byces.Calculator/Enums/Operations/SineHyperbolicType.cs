using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class SineHyperbolicType : OperationType
    {
        internal SineHyperbolicType(int value) : base("SineHyperbolic", value) { }

        internal override string StringRepresentation => "SINH";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Sinh(number);
    }
}