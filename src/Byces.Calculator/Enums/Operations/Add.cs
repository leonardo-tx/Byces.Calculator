namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Add : OperationType
    {
        protected override int Value => 0;
        protected override string StringRepresentation => "ADD";
        protected override char CharRepresentation => '+';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber + secondNumber;
    }
}