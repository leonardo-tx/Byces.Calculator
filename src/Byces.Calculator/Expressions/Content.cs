using Byces.Calculator.Enums;
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

        internal List<double> Numbers { get; }

        internal List<Operation> Operations { get; }

        internal List<Function> Functions { get; }

        internal void Build(ReadOnlySpan<char> expressionSpan)
        {
            ContentBuilder.Build(this, expressionSpan);
        }

        internal void Clear()
        {
            Numbers.Clear();
            Operations.Clear();
            Functions.Clear();
        }

        internal void Process()
        {
            CalculatePriorities();
            CalculateFunctions(0);
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

                CalculateFunctions(priority);
                CalculateOperationsInOrder(firstIndex, lastIndex + 1);

                firstIndex = -1;

                if (Operations.Count == 0) return;
                int newPriority = Operations.MaxPriority();

                if (newPriority == priority) continue;
                if (newPriority == 0) return;

                priority = newPriority; i = -1;
            }
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

        private void CalculateOperationsInOrder(int? firstIndex = null, int? count = null)
        {
            CalculateOperations(OperationPriorityType.First, firstIndex, ref count);
            CalculateOperations(OperationPriorityType.Second, firstIndex, ref count);
            CalculateOperations(OperationPriorityType.Third, firstIndex, ref count);
        }

        private void CalculateOperations(OperationPriorityType operationPriority, int? firstIndex, ref int? count)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Count); i++)
            {
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

        private void SetFunctionToIndex(int oldIndex, int newIndex)
        {
            for (int i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].NumberIndex != oldIndex) continue;
                Functions[i] = new Function(newIndex, Functions[i].Value, Functions[i].Priority);
            }
        }
    }
}