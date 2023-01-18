﻿using Byces.Calculator.Enums;
using Byces.Calculator.Extensions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly ref struct Content
    {
        internal Content(Span<Number?> numbers, Span<Operation?> operations, Span<SelfOperation?> selfOperations)
        {
            Numbers = numbers;
            Operations = operations;
            SelfOperations = selfOperations;
        }

        internal Span<Number?> Numbers { get; }

        internal Span<Operation?> Operations { get; }

        internal Span<SelfOperation?> SelfOperations { get; }

        private static readonly ReadOnlyMemory<int> FirstPriority = new int[2] { OperationType.Power, OperationType.Root };
        private static readonly ReadOnlyMemory<int> SecondPriority = new int[3] { OperationType.Multiply, OperationType.Divide, OperationType.Modulus };
        private static readonly ReadOnlyMemory<int> ThirdPriority = new int[2] { OperationType.Add, OperationType.Subtract };

        internal void Build(ReadOnlySpan<char> expressionSpan)
        {
            var contentBuilder = new ContentBuilder(expressionSpan);
            contentBuilder.Build(this);
        }

        internal void Process()
        {
            CalculatePriorities();
            CalculateSelfOperations(0);
            CalculateOperationsInOrder();
        }

        private void CalculatePriorities()
        {
            if (Operations.Length == 0) return;

            int priority = Operations.MaxPriority();
            if (priority == 0) return;

            for (int i = 0, firstIndex = -1; i < Operations.Length; i++)
            {
                if (Operations[i] == null) continue;
                if (Operations[i]!.Value.Priority != priority) continue;
                if (firstIndex == -1) firstIndex = i;

                if (i + 1 != Operations.Length && Operations[i]!.Value.Priority == ((Operations[i + 1] != null) ? Operations[i + 1]!.Value.Priority : -1)) continue;
                int lastIndex = i;

                CalculateSelfOperations(priority);
                CalculateOperationsInOrder(firstIndex, lastIndex + 1);

                firstIndex = -1;

                if (Operations.Length == 0) return;
                int newPriority = Operations.MaxPriority();

                if (newPriority == priority) continue;
                if (newPriority == 0) return;
                
                priority = newPriority; i = -1; continue;
            }
        }

        private void CalculateSelfOperations(int minPriority)
        {
            for (int i = SelfOperations.Length - 1; i >= 0; i--)
            {
                if (SelfOperations[i] == null) continue;
                if (SelfOperations[i]!.Value.Operation.Priority < minPriority) continue;

                int maxPriority = SelfOperations.MaxPriority();
                while (maxPriority > minPriority)
                {
                    CalculateSelfOperations(maxPriority);
                    maxPriority = SelfOperations.MaxPriority();
                }
                int numberIndex = SelfOperations[i]!.Value.NumberIndex;
                double result = SelfOperations[i]!.Value.Operation.Operate(Numbers[numberIndex]!.Value.Value);

                SelfOperations[i] = null;
                Numbers[numberIndex] = new Number(result);
            }
        }

        private void CalculateSelfOperations(int priority, int? firstIndex = null)
        {
            for (int i = SelfOperations.Length - 1; i >= 0; i--)
            {
                if (SelfOperations[i] == null) continue;
                if (SelfOperations[i]!.Value.NumberIndex < firstIndex) continue;
                if (SelfOperations[i]!.Value.Operation.Priority != priority && priority != 0) continue;

                int numberIndex = SelfOperations[i]!.Value.NumberIndex;
                double result = SelfOperations[i]!.Value.Operation.Operate(Numbers[numberIndex]!.Value.Value);
                
                SelfOperations[i] = null;
                Numbers[numberIndex] = new Number(result);
            }
        }

        private void CalculateOperationsInOrder(int? firstIndex = null, int? count = null)
        {
            CalculateOperations(FirstPriority.Span, firstIndex, count);
            CalculateOperations(SecondPriority.Span, firstIndex, count);
            CalculateOperations(ThirdPriority.Span, firstIndex, count);
        }

        private void CalculateOperations(ReadOnlySpan<int> operations, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Length); i++)
            {
                if (Operations[i] == null) continue;
                
                Operation operation = Operations[i]!.Value;
                if (!operations.CustomContains(operation.Value)) continue;

                int firstNumberIndex = GetFirstNumberIndex(i), secondNumberIndex = GetSecondNumberIndex(i);
                double result = operation.Operate(Numbers[firstNumberIndex]!.Value.Value, Numbers[secondNumberIndex]!.Value.Value);

                Operations[i] = null;
                Numbers[secondNumberIndex] = null;
                Numbers[firstNumberIndex] = new Number(result);

                SetSelfOperationsToIndex(secondNumberIndex, firstNumberIndex);
                count--;
            }
        }

        private int GetFirstNumberIndex(int operationIndex)
        {
            for (int i = operationIndex; i >= 0; i--)
            {
                if (Numbers[i] == null) continue;
                return i;
            }
            throw new IndexOutOfRangeException();
        }

        private int GetSecondNumberIndex(int operationIndex)
        {
            for (int i = operationIndex + 1; i < Numbers.Length; i++)
            {
                if (Numbers[i] == null) continue;
                return i;
            }
            throw new IndexOutOfRangeException();
        }

        private void SetSelfOperationsToIndex(int oldIndex, int newIndex)
        {
            for (int i = 0; i < SelfOperations.Length; i++)
            {
                if (SelfOperations[i] == null) continue;
                if (SelfOperations[i]!.Value.NumberIndex != oldIndex) continue;
                
                SelfOperations[i] = new SelfOperation(SelfOperations[i]!.Value.Operation, newIndex);
            }
        }
    }
}