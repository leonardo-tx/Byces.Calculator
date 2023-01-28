using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Divide : OperationType
    {
        protected override int Value => 3;
        internal override string StringRepresentation => "DIV";
        internal override char CharRepresentation => '/';
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override double Operate(double firstNumber, double secondNumber)
        {
            if (secondNumber == 0) throw new ArithmeticExpressionException("Attempted to divide by zero.");
            return firstNumber / secondNumber;
        }
    }
}