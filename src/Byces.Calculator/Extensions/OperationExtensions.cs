using Byces.Calculator.Enums;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Extensions
{
    internal static class OperationExtensions
    {
        internal static double Operate(this Operation operation, double firstNumber, double secondNumber)
        {
            return operation switch
            {
                Operation.Root => Math.Pow(secondNumber, 1 / firstNumber),
                Operation.Power => Math.Pow(firstNumber, secondNumber),
                Operation.Multiply => firstNumber * secondNumber,
                Operation.Divide => firstNumber / secondNumber,
                Operation.Add => firstNumber + secondNumber,
                Operation.Subtract => firstNumber - secondNumber,
                _ => throw new ArgumentException("Invalid operator."),
            };
        }

        internal static double Operate(this Operation operation, double number)
        {
            return operation switch
            {
                Operation.Factorial => CalculateFactorial(number),
                Operation.Root => Math.Sqrt(number),
                _ => throw new ArgumentException("Invalid self operator."),
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