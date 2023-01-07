using Byces.Calculator.Enums;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Extensions.Content
{
    internal static class OperationTypeExtensions
    {
        internal static double Operate(this OperationType operation, double firstNumber, double secondNumber)
        {
            return operation switch
            {
                OperationType.Root => Math.Pow(secondNumber, 1 / firstNumber),
                OperationType.Power => Math.Pow(firstNumber, secondNumber),
                OperationType.Multiply => firstNumber * secondNumber,
                OperationType.Divide => firstNumber / secondNumber,
                OperationType.Add => firstNumber + secondNumber,
                OperationType.Subtract => firstNumber - secondNumber,
                _ => throw new NotSupportedException("Not supported operation."),
            };
        }

        internal static double Operate(this OperationType operation, double number)
        {
            return operation switch
            {
                OperationType.Factorial => CalculateFactorial(number),
                OperationType.Root => Math.Sqrt(number),
                _ => throw new NotSupportedException("Not supported operation."),
            };
        }

        private static double CalculateFactorial(double number)
        {
            if (number < 0) return double.NaN;

            double difference = number - (int)number;
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }
    }
}