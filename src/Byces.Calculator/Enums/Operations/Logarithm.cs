using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Logarithm : OperationType
    {
        protected override int Value => 7;
        internal override string StringRepresentation => "LOG";
        internal override char CharRepresentation => default;
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override double Operate(double firstNumber, double secondNumber)
        {
            if (firstNumber <= 1) throw new ArithmeticExpressionException($"Attempted to base {firstNumber} on a logarithm");
            if (secondNumber <= 0) throw new ArithmeticExpressionException($"Attempted to log (base {firstNumber}) of {secondNumber}");
            return Math.Log(secondNumber, firstNumber);
        }
    }
}