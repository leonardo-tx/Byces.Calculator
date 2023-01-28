using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Root : OperationType
    {
        protected override int Value => 6;
        internal override string StringRepresentation => "RT";
        internal override char CharRepresentation => '√';
        internal override OperationPriorityType Priority => OperationPriorityType.First;

        internal override double Operate(double firstNumber, double secondNumber)
        {
            if (firstNumber < 0) throw new ArithmeticExpressionException("Attempted to make a negative root.");
            if (secondNumber < 0 && firstNumber % 2 == 0) throw new ArithmeticExpressionException("Attempted to use an even number to take the root of a negative number.");

            double result;
            if (secondNumber < 0)
            {
                result = Math.Pow(secondNumber * -1, 1.0 / firstNumber) * -1;
            }
            else
            {
                result = Math.Pow(secondNumber, 1.0 / firstNumber);
            }
            return Math.Round(result, 14);
        }
    }
}