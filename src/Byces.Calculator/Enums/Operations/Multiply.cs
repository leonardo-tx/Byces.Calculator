namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Multiply : OperationType
    {
        protected override int Value => 2;
        protected override string StringRepresentation => "MUL";
        protected override char CharRepresentation => '*';
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override double Operate(double firstNumber, double secondNumber) => firstNumber * secondNumber;
    }
}