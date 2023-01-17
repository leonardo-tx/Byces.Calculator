using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class TangentType : OperationType
    {
        internal TangentType(int value) : base("Tangent", value) { }

        internal override string StringRepresentation => "TAN";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Tan(number);
    }
}