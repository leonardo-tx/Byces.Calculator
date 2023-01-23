namespace Byces.Calculator.Enums.Operations
{
    internal sealed class AddType : OperationType
    {
        protected override int Value => 0;
        internal override string StringRepresentation => "ADD";
        internal override char CharRepresentation => '+';

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber + secondNumber;
    }
}