namespace Byces.Calculator.Enums.Operations
{
    internal sealed class SubtractType : OperationType
    {
        protected override int Value => 1;
        internal override string StringRepresentation => "SUB";
        internal override char CharRepresentation => '-';

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber - secondNumber;
    }
}