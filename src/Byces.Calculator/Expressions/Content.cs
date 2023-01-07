using Byces.Calculator.Enums;
using Byces.Calculator.Extensions.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Content
    {
        internal Content(IList<Number> numbers, IList<Operation> operations)
        {
            Numbers = numbers;
            Operations = operations;
        }

        internal static readonly Content Default = new Content(new List<Number>(1) { new Number(0) }, Array.Empty<Operation>());

        internal IList<Number> Numbers { get; }

        internal IList<Operation> Operations { get; }

        private static readonly OperationType[] FirstPriority = new OperationType[2] { OperationType.Power, OperationType.Root };
        private static readonly OperationType[] SecondPriority = new OperationType[2] { OperationType.Multiply, OperationType.Divide };
        private static readonly OperationType[] ThirdPriority = new OperationType[2] { OperationType.Add, OperationType.Subtract };

        internal void Process()
        {
            CalculatePriorities();
            CalculateSelfOperators(0);
            CalculateOperatorsInOrder();
        }

        private void CalculatePriorities()
        {
            if (Operations.Count == 0) return;

            int priority = Operations.MaxPriority();
            if (priority == 0) return;

            int firstIndex = -1;
            for (int i = 0; i < Operations.Count; i++)
            {
                if (Operations[i].Priority != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != Operations.Count && Operations[i].Priority == Operations[i + 1].Priority) continue;
                int lastIndex = i;

                CalculateSelfOperators(priority, firstIndex);
                int removedCount = lastIndex + 1 - (int)CalculateOperatorsInOrder(firstIndex, lastIndex + 1)!;

                i -= removedCount + 1;
                firstIndex = -1;

                if (Operations.Count == 0) return;
                priority = Operations.MaxPriority();

                if (priority == 0) return;
                if (i < 0) i = 0;
            }
        }

        private void CalculateSelfOperators(int priority, int? firstIndex = null)
        {
            for (int i = firstIndex ?? 0; i < Numbers.Count; i++)
            {
                for (int j = 0; j < Numbers[i].Operations.Count; j++)
                {
                    if (Numbers[i].Operations[j].Priority != priority) continue;

                    double result = Numbers[i].Operations[j].Value.Operate(Numbers[i].Value);
                    Numbers[i].Operations.RemoveAt(j);
                    var selfOperations = Numbers[i].Operations;

                    Numbers.RemoveAt(i);
                    Numbers.Insert(i, new Number(result, selfOperations));
                    j--;
                }
            }
        }

        private int? CalculateOperatorsInOrder(int? firstIndex = null, int? count = null)
        {
            count = CalculateOperations(FirstPriority, firstIndex, count);
            count = CalculateOperations(SecondPriority, firstIndex, count);
            return CalculateOperations(ThirdPriority, firstIndex, count);
        }

        private int? CalculateOperations(OperationType[] operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Count); i++)
            {
                if (!operations.Contains(Operations[i].Value)) continue;
                double result = Operations[i].Value.Operate(Numbers[i].Value, Numbers[i + 1].Value);

                Operations.RemoveAt(i);
                IList<Operation> selfOperations = Numbers[i + 1].Operations;

                for (int j = 0; j < 2; j++) Numbers.RemoveAt(i);
                Numbers.Insert(i, new Number(result, selfOperations));

                i--; count--;
            }
            return count;
        }
    }
}