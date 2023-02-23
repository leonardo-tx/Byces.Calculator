namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Subtract : OperationType
    {
        protected override int Value => 1;
        protected override string StringRepresentation => "SUB";
        protected override char CharRepresentation => '-';
        internal override OperationPriorityType Priority => OperationPriorityType.Third;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber - secondNumber;
    }
}