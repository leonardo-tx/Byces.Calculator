using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;

namespace Byces.Calculator.Expressions
{
    internal sealed class Content
    {
        public Content()
        {
            Numbers = new List<double>();
            Operations = new List<Operation>();
            Functions = new List<Function>();
        }

        private const int StackAllocationLimit = 1024;

        internal List<double> Numbers { get; }

        internal List<Operation> Operations { get; }

        internal List<Function> Functions { get; }

        internal void Build(ReadOnlySpan<char> expressionSpan, bool hasWhiteSpaceRemover)
        {
            ContentBuilder builder = new ContentBuilder(hasWhiteSpaceRemover);
            builder.Build(this, expressionSpan);
        }

        internal void Clear()
        {
            Numbers.Clear();
            Operations.Clear();
            Functions.Clear();
        }

        internal void Process()
        {
            int priority = Operations.MaxPriority();
            while (priority >= 0)
            {
                CalculateFunctions(priority);
                CalculateOperationsInOrder(priority);
                priority = Operations.MaxPriority();
            }
            CalculateFunctions(0);
        }

        private void CalculateFunctions(int minPriority)
        {
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                if (Functions[i].Priority < minPriority) continue;

                int maxPriority = Functions.MaxPriority();
                while (maxPriority > minPriority)
                {
                    CalculateFunctions(maxPriority);
                    maxPriority = Functions.MaxPriority();
                }
                while (i >= Functions.Count) i--;

                int numberIndex = Functions[i].NumberIndex;
                double result = Functions[i].Operate(Numbers[numberIndex]);

                Functions.RemoveAt(i);
                Numbers[numberIndex] = result;
            }
        }

        private void CalculateOperationsInOrder(int priority)
        {
            CalculateOperations(OperationPriorityType.First, priority);
            CalculateOperations(OperationPriorityType.Second, priority);
            CalculateOperations(OperationPriorityType.Third, priority);
            FindSemiColon(priority);
        }

        private void CalculateOperations(OperationPriorityType operationPriority, int priority)
        {
            int count = Operations.Count;
            for (int i = 0; i < count; i++)
            {
                if (Operations[i].Priority != priority) continue;

                OperationType operationType = (OperationType)Operations[i].Value;
                if (operationPriority != operationType.Priority) continue;

                double result = operationType.Operate(Numbers[i], Numbers[i + 1]);

                Numbers[i] = result;
                Operations.RemoveAt(i);
                Numbers.RemoveAt(i + 1);

                SetFunctionToIndex(i + 1, i);
                count--; i--;
            }
        }

        private void FindSemiColon(int priority)
        {
            int? firstIndex = null, lastIndex = null;
            for (int i = 0; i < Operations.Count; i++)
            {
                if (firstIndex.HasValue && Operations[i].Priority != priority) { lastIndex = i; break; }
                if (Operations[i].Priority == priority && !firstIndex.HasValue) { firstIndex = i; continue; }
            }
            if (!firstIndex.HasValue) return;
            CalculateMultipleArgsFunction((int)firstIndex!, (lastIndex.HasValue) ? (int)lastIndex - (int)firstIndex + 1 : Numbers.Count - (int)firstIndex);
        }

        private void CalculateMultipleArgsFunction(int firstIndex, int count)
        {
            int functionIndex = FindFunctionIndex(firstIndex);
            if (functionIndex == -1) throw new MissingFunctionExpressionException();

            Span<double> numbers = (StackAllocationLimit >= count) ? stackalloc double[count] : new double[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = Numbers[firstIndex + i];
            }
            double result = Functions[functionIndex].Operate(numbers);

            Numbers[firstIndex] = result;
            Operations.RemoveRange(firstIndex, count - 1);
            Numbers.RemoveRange(firstIndex + 1, count - 1);
            Functions.RemoveAt(functionIndex);
        }

        private int FindFunctionIndex(int numberIndex)
        {
            int count = Functions.Count;
            for (int i = 0; i < count; i++)
            {
                if (Functions[i].NumberIndex != numberIndex) continue;
                return i;
            }
            return -1;
        }

        private void SetFunctionToIndex(int oldIndex, int newIndex)
        {
            int count = Functions.Count;
            for (int i = 0; i < count; i++)
            {
                if (Functions[i].NumberIndex != oldIndex) continue;
                Functions[i] = new Function(newIndex, Functions[i].Value, Functions[i].Priority);
            }
        }
    }
}