using Byces.Calculator.Enums;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Content
    {
        internal Content(IList<Number> numbers, IList<Operation> operations, IList<int> priorities)
        {
            Numbers = numbers;
            Operations = operations;
            Priorities = priorities;
        }

        internal static readonly Content Empty = new Content(new List<Number>(1) { new Number(0) }, Array.Empty<Operation>(), Array.Empty<int>());

        internal IList<Number> Numbers { get; }

        internal IList<Operation> Operations { get; }

        internal IList<int> Priorities { get; }

        private static readonly Operation[] FirstPriority = new Operation[2] { Operation.Power, Operation.Root };
        private static readonly Operation[] SecondPriority = new Operation[2] { Operation.Multiply, Operation.Divide };
        private static readonly Operation[] ThirdPriority = new Operation[2] { Operation.Add, Operation.Subtract };

        internal void Process()
        {
            CalculatePriorities();
            CalculateSelfOperators(0);
            CalculateOperatorsInOrder();
        }

        private void CalculatePriorities()
        {
            if (Priorities.Count == 0) return;
#if NET7_0_OR_GREATER
            int priority = Priorities.Max();
#else
            int priority = Priorities.CustomMax();
#endif
            if (priority == 0) return;

            int firstIndex = -1;
            for (int i = 0; i < Priorities.Count; i++)
            {
                if (Priorities[i] != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != Priorities.Count && Priorities[i] == Priorities[i + 1]) continue;
                int lastIndex = i;

                CalculateSelfOperators(priority, firstIndex);
                int removedCount = lastIndex + 1 - (int)CalculateOperatorsInOrder(firstIndex, lastIndex + 1)!;

                i -= removedCount + 1;
                firstIndex = -1;

                if (Priorities.Count == 0) return;
#if NET7_0_OR_GREATER
                priority = Priorities.Max();
#else
                priority = Priorities.CustomMax();
#endif
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

                    double result = Numbers[i].Operations[j].Operation.Operate(Numbers[i].Value);
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

        private int? CalculateOperations(Operation[] operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Count); i++)
            {
                if (!operations.Contains(Operations[i])) continue;
                double result = Operations[i].Operate(Numbers[i].Value, Numbers[i + 1].Value);

                Operations.RemoveAt(i);
                Priorities.RemoveAt(i);
                IList<SelfOperation> selfOperations = Numbers[i + 1].Operations;

                for (int j = 0; j < 2; j++) Numbers.RemoveAt(i);
                Numbers.Insert(i, new Number(result, selfOperations));

                i--; count--;
            }
            return count;
        }
    }
}