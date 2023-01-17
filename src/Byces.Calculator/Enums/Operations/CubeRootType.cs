using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class CubeRootType : OperationType
    {
        internal CubeRootType(int value) : base("CubeRoot", value) { }

        internal override string StringRepresentation => "CBRT";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Cbrt(number);
    }
}