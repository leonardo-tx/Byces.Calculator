namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Multiply : OperationType
    {
        protected override int Value => 2;
        internal override string StringRepresentation => "MUL";
        internal override char CharRepresentation => '*';
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber * secondNumber;
    }
}