using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class PowerType : OperationType
    {
        protected override int Value => 5;
        internal override string StringRepresentation => "POW";
        internal override char CharRepresentation => '^';

        internal override double Operate(double firstNumber, double secondNumber) => Math.Pow(firstNumber, secondNumber);
    }
}