namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Add : OperationType
    {
        protected override int Value => 0;
        internal override string StringRepresentation => "ADD";
        internal override char CharRepresentation => '+';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber + secondNumber;
    }
}