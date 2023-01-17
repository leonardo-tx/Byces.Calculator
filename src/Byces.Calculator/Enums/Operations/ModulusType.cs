using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class ModulusType : OperationType
    {
        internal ModulusType(int value) : base("Modulus", value) { }

        internal override string StringRepresentation => "MOD";
        internal override char CharRepresentation => '%';
        internal override OperationCategory Category => OperationCategory.None;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber % secondNumber;

        internal override double Operate(double number) => throw new NotSupportedException();
    }
}