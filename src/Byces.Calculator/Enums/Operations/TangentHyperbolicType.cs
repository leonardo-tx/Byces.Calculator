using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class TangentHyperbolicType : OperationType
    {
        internal TangentHyperbolicType(int value) : base("TangentHyperbolic", value) { }

        internal override string StringRepresentation => "TANH";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Tanh(number);
    }
}