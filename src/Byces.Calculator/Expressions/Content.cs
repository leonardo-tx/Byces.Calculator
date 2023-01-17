using Byces.Calculator.Enums;
using Byces.Calculator.Extensions.Content;
using Byces.Calculator.Extensions.ExpressionBuilder;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

        private static readonly ReadOnlyMemory<int> FirstPriority = new int[2] { OperationType.Power, OperationType.Root };
        private static readonly ReadOnlyMemory<int> SecondPriority = new int[3] { OperationType.Multiply, OperationType.Divide, OperationType.Modulus };
        private static readonly ReadOnlyMemory<int> ThirdPriority = new int[2] { OperationType.Add, OperationType.Subtract };

        internal void Process()
        {
            CalculatePriorities();
            CalculateSelfOperations(0);
            CalculateOperationsInOrder();
        }

        private void CalculatePriorities()
        {
            if (Operations.Count == 0) return;

            int priority = Operations.MaxPriority();
            if (priority == 0) return;

            for (int i = 0, firstIndex = -1; i < Operations.Count; i++)
            {
                if (Operations[i].Priority != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != Operations.Count && Operations[i].Priority == Operations[i + 1].Priority) continue;
                int lastIndex = i;

                CalculateSelfOperations(priority, firstIndex);
                int removedCount = lastIndex + 1 - (int)CalculateOperationsInOrder(firstIndex, lastIndex + 1)!;

                i -= removedCount + 1;
                firstIndex = -1;

                if (Operations.Count == 0) return;
                priority = Operations.MaxPriority();

                if (priority == 0) return;
                if (i < 0) i = 0;
            }
        }

        private void CalculateSelfOperations(int priority, int? firstIndex = null)
        {
            for (int i = firstIndex ?? 0; i < Numbers.Count; i++)
            {
                for (int j = Numbers[i].Operations.Count - 1; j >= 0; j--)
                {
                    if (Numbers[i].Operations[j].Priority != priority && priority != 0) continue;

                    double result = Numbers[i].Operations[j].Operate(Numbers[i].Value);
                    Numbers[i].Operations.RemoveAt(j);
                    var selfOperations = Numbers[i].Operations;

                    Numbers.RemoveAt(i);
                    Numbers.Insert(i, new Number(result, selfOperations));
                }
            }
        }

        private int? CalculateOperationsInOrder(int? firstIndex = null, int? count = null)
        {
            count = CalculateOperations(FirstPriority.Span, firstIndex, count);
            count = CalculateOperations(SecondPriority.Span, firstIndex, count);
            return CalculateOperations(ThirdPriority.Span, firstIndex, count);
        }

        private int? CalculateOperations(ReadOnlySpan<int> operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Count); i++)
            {
#if NET5_0_OR_GREATER
                ReadOnlySpan<Operation> Operations = CollectionsMarshal.AsSpan((List<Operation>)this.Operations);
#endif
                if (!operations.CustomContains(Operations[i].Value)) continue;
#if NET5_0_OR_GREATER
                ReadOnlySpan<Number> Numbers = CollectionsMarshal.AsSpan((List<Number>)this.Numbers);
#endif
                double result = Operations[i].Operate(Numbers[i].Value, Numbers[i + 1].Value);
                int totalSelfOperations = Numbers[i + 1].Operations.Count + Numbers[i].Operations.Count;

                IList<Operation> selfOperations;
                if (totalSelfOperations != 0) selfOperations = new List<Operation>(totalSelfOperations);
                else selfOperations = Array.Empty<Operation>();

                for (int j = 0; j < Numbers[i].Operations.Count; j++) selfOperations.Add(Numbers[i].Operations[j]);
                for (int j = 0; j < Numbers[i + 1].Operations.Count; j++) selfOperations.Add(Numbers[i + 1].Operations[j]);
                for (int j = 0; j < 2; j++) this.Numbers.RemoveAt(i);

                this.Numbers.Insert(i, new Number(result, selfOperations));
                this.Operations.RemoveAt(i);

                i--; count--;
            }
            return count;
        }
    }
}