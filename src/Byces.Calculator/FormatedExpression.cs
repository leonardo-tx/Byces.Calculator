using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Byces.Calculator
{
    public readonly struct FormatedExpression
    {
        internal FormatedExpression(Content content, bool isValid, string? errorMessage = null)
        {
            Content = content;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        internal FormatedExpression(string errorMessage)
        {
            Content = Content.Empty;
            IsValid = false;
            ErrorMessage = errorMessage;
        }

        public static readonly FormatedExpression Empty = new FormatedExpression(Content.Empty, true, null);

        public bool IsValid { get; }

        public string? ErrorMessage { get; }

        internal Content Content { get; }

        private static readonly Operation[] FirstPriority = new Operation[2] { Operation.Power, Operation.Root };
        private static readonly Operation[] SecondPriority = new Operation[2] { Operation.Multiply, Operation.Divide };
        private static readonly Operation[] ThirdPriority = new Operation[2] { Operation.Add, Operation.Subtract };

        /// <summary>
        /// This takes an mathematic <see cref="FormatedExpression"/> and returns a <see cref="double"/> result.
        /// </summary>
        /// <returns>The calculation result.</returns>
        public double Calculate()
        {
            if (!IsValid) throw new ArgumentException($"This {nameof(FormatedExpression)} instance is invalid, and cannot be used to calculate");

            CalculateAll();
            return Content.Numbers[0].Value;
        }

        private void CalculateAll()
        {
            CalculatePriorities();
            CalculateSelfOperators(0);
            CalculateOperatorsInOrder();
        }

        private void CalculatePriorities()
        {
            if (Content.Priorities.Count == 0) return;
            for (int i = 0; i < Content.Priorities.Count; i++)
            {
                if (Content.Priorities[i] > 0) break;
                if (Content.Priorities.Count == i + 1) return;
            }
            int priority = Content.Priorities.Max();
            int firstIndex = -1;
            for (int i = 0; i < Content.Priorities.Count; i++)
            {
                if (Content.Priorities[i] != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != Content.Priorities.Count && Content.Priorities[i] == Content.Priorities[i + 1]) continue;
                var lastIndex = i;

                CalculateSelfOperators(priority);
                int removedCount = lastIndex + 1 - (int)CalculateOperatorsInOrder(firstIndex, lastIndex + 1)!;

                i -= removedCount + 1;
                firstIndex = -1;

                if (Content.Priorities.Count == 0) return;
                priority = Content.Priorities.Max();
                if (priority == 0) return;
                if (i < 0) i = 0;
            }
        }

        private void CalculateSelfOperators(int priority)
        {
            for (int i = 0; i < Content.Numbers.Count; i++)
            {
                for (int j = 0; j < Content.Numbers[i].Operations.Count; j++)
                {
                    if (Content.Numbers[i].Operations[j].Priority != priority) continue;

                    double result = Operate(Content.Numbers[i].Operations[j].Operation, Content.Numbers[i].Value);
                    Content.Numbers[i].Operations.RemoveAt(j);
                    var selfOperations = Content.Numbers[i].Operations;

                    Content.Numbers.RemoveAt(i);
                    Content.Numbers.Insert(i, new Number(result, selfOperations));
                    j--;
                    if (j + 1 == Content.Numbers[i].Operations.Count)
                    {
                        if (Content.Numbers.Count == i + 1) return;
                        while (Content.Numbers[i + 1].Operations.Count == 0)
                        {
                            i++;
                            if (Content.Numbers.Count == i + 1) return;
                        }
                        if (Content.Numbers[i + 1].Operations[0].Priority != priority) return;
                    }
                }
            }
        }

        private int? CalculateOperatorsInOrder(int? firstIndex = null, int? count = null)
        {
            count = CalculateOperations(FirstPriority, firstIndex, count);
            count = CalculateOperations(SecondPriority, firstIndex, count);
            return CalculateOperations(ThirdPriority, firstIndex, count);
        }

        private int? CalculateOperations(Operation[] operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Content.Operations.Count); i++)
            {
                if (!operations.Contains(Content.Operations[i])) continue;
                double result = Operate(Content.Operations[i], Content.Numbers[i].Value, Content.Numbers[i + 1].Value);

                Content.Operations.RemoveAt(i);
                Content.Priorities.RemoveAt(i);
                IList<SelfOperation> selfOperations = Content.Numbers[i + 1].Operations;

                for (int j = 0; j < 2; j++) Content.Numbers.RemoveAt(i);
                Content.Numbers.Insert(i, new Number(result, selfOperations));

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

        private static double Operate(Operation symbol, double number)
        {
            return symbol switch
            {
                Operation.Factorial => CalculateFactorial(number),
                Operation.Root => Math.Sqrt(number),
                _ => throw new ArgumentException("Invalid operator."),
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