namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Subtract : OperationType
    {
        protected override int Value => 1;
        internal override string StringRepresentation => "SUB";
        internal override char CharRepresentation => '-';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber - secondNumber;
    }
}