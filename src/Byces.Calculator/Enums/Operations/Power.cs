using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Power : OperationType
    {
        protected override int Value => 5;
        protected override string StringRepresentation => "POW";
        protected override char CharRepresentation => '^';
        internal override OperationPriorityType Priority => OperationPriorityType.First;

        internal override double Operate(double firstNumber, double secondNumber) => Math.Pow(firstNumber, secondNumber);
    }
}