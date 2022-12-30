using System;
using System.Collections.Generic;
using System.Linq;
using Byces.Calculator.Enums;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the calculation of an expression.
    /// </summary>
    public static class Calculator
    {
        private static readonly Operation[] FirstPriority = new Operation[2] { Operation.Power, Operation.Root };
        private static readonly Operation[] SecondPriority = new Operation[2] { Operation.Multiply, Operation.Divide };
        private static readonly Operation[] ThirdPriority = new Operation[2] { Operation.Add, Operation.Subtract };

        /// <summary>
        /// This takes an mathematic <see cref="Expression"/> and returns a <see cref="double"/> result.
        /// </summary>
        /// <param name="expression">The formatted expression</param>
        /// <returns>The calculation result.</returns>
        public static double Calculate(Expression expression)
        {
            if (!expression.IsValid) throw new ArgumentException("The given ExpressionResult is invalid, and cannot be used to calculate");
            
            CalculateAll(expression.NumbersList, expression.OperatorsList, expression.PrioritiesList);
            return expression.NumbersList[0];
        }

        private static void CalculateAll(IList<double> numbersList, IList<Operation> operatorsList, IList<int> prioritiesList)
        {
            CalculatePriorities(numbersList, operatorsList, prioritiesList);
            CalculateOperatorsInOrder(numbersList, operatorsList, prioritiesList);
        }

        private static void CalculateOperatorsInOrder(IList<double> numbersList, IList<Operation> operatorsList, IList<int> prioritiesList, int? firstIndex = null, int? count = null)
        {
            count = CalculateOperators(numbersList, operatorsList, prioritiesList, FirstPriority, firstIndex, count);
            count = CalculateOperators(numbersList, operatorsList, prioritiesList, SecondPriority, firstIndex, count);
            CalculateOperators(numbersList, operatorsList, prioritiesList, ThirdPriority, firstIndex, count);
        }

        private static void CalculatePriorities(IList<double> numbersList, IList<Operation> operatorsList, IList<int> prioritiesList)
        {
            if (prioritiesList.Count == 0) return;
            for (int i = 0; i < prioritiesList.Count; i++)
            {
                if (prioritiesList[i] > 0) break;
                if (prioritiesList.Count == i + 1) return;
            }
            int priority = prioritiesList.Max();
            int firstIndex = -1;
            for (int i = 0; i < prioritiesList.Count; i++)
            {
                if (prioritiesList[i] != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != prioritiesList.Count && prioritiesList[i] == prioritiesList[i + 1]) continue;
                var lastIndex = i;

                CalculateOperatorsInOrder(numbersList, operatorsList, prioritiesList, firstIndex, lastIndex + 1);
                
                firstIndex = -1;

                if (prioritiesList.Count == 0) return;
                priority = prioritiesList.Max();
                if (priority == 0) return;
            }
        }

        private static int? CalculateOperators(IList<double> numbersList, IList<Operation> operatorsList, IList<int> prioritiesList, Operation[] operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? operatorsList.Count); i++)
            {
                if (!operations.Contains(operatorsList[i])) continue;
                double result = Operate(operatorsList[i], numbersList[i], numbersList[i + 1]);

                operatorsList.RemoveAt(i);
                prioritiesList.RemoveAt(i);
                for (int j = 0; j < 2; j++) numbersList.RemoveAt(i);
                numbersList.Insert(i, result);

                i--; count--;
            }
            return count;
        }

        private static double Operate(Operation symbol, double firstNumber, double secondNumber)
        {
            return symbol switch
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
    }
}