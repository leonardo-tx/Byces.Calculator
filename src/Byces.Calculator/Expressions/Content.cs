using Byces.Calculator.Enums;
using Byces.Calculator.Extensions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly ref struct Content
    {
        internal Content(Span<double?> numbers, Span<Operation?> operations, Span<Function?> functions)
        {
            Numbers = numbers;
            Operations = operations;
            Functions = functions;
        }

        internal Span<double?> Numbers { get; }

        internal Span<Operation?> Operations { get; }

        internal Span<Function?> Functions { get; }

        internal void Build(ReadOnlySpan<char> expressionSpan, BuilderInfo builderInfo)
        {
            var contentBuilder = new ContentBuilder(expressionSpan, builderInfo);
            contentBuilder.Build(this);
        }

        internal void Process()
        {
            CalculatePriorities();
            CalculateFunctions(0);
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

                CalculateFunctions(priority);
                CalculateOperationsInOrder(firstIndex, lastIndex + 1);

                firstIndex = -1;

                if (Operations.Length == 0) return;
                int newPriority = Operations.MaxPriority();

                if (newPriority == priority) continue;
                if (newPriority == 0) return;
                
                priority = newPriority; i = -1; continue;
            }
        }

        private void CalculateFunctions(int minPriority)
        {
            for (int i = Functions.Length - 1; i >= 0; i--)
            {
                if (Functions[i] == null) continue;
                if (Functions[i]!.Value.Priority < minPriority) continue;

                int maxPriority = Functions.MaxPriority();
                while (maxPriority > minPriority)
                {
                    CalculateFunctions(maxPriority);
                    maxPriority = Functions.MaxPriority();
                }
                if (Functions[i] == null) continue;
                int numberIndex = Functions[i]!.Value.NumberIndex;
                double result = Functions[i]!.Value.Operate(Numbers[numberIndex]!.Value);

                Functions[i] = null;
                Numbers[numberIndex] = result;
            }
        }

        private void CalculateOperationsInOrder(int? firstIndex = null, int? count = null)
        {
            CalculateOperations(OperationPriorityType.First, firstIndex, count);
            CalculateOperations(OperationPriorityType.Second, firstIndex, count);
            CalculateOperations(OperationPriorityType.Third, firstIndex, count);
        }

        private void CalculateOperations(OperationPriorityType operationPriority, int? firstIndex = null, int? count = null)
        {
            for (int i = firstIndex ?? 0; i < (count ?? Operations.Length); i++)
            {
                if (Operations[i] == null) continue;

                OperationType operationType = OperationType.GetOperation(Operations[i]!.Value.Value);
                if (operationPriority != operationType.Priority) continue;

                int firstNumberIndex = GetFirstNumberIndex(i), secondNumberIndex = GetSecondNumberIndex(i);
                double result = operationType.Operate(Numbers[firstNumberIndex]!.Value, Numbers[secondNumberIndex]!.Value);

                Operations[i] = null;
                Numbers[secondNumberIndex] = null;
                Numbers[firstNumberIndex] = result;

                SetFunctionToIndex(secondNumberIndex, firstNumberIndex);
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

        private void SetFunctionToIndex(int oldIndex, int newIndex)
        {
            for (int i = 0; i < Functions.Length; i++)
            {
                if (Functions[i] == null) continue;
                if (Functions[i]!.Value.NumberIndex != oldIndex) continue;
                
                Functions[i] = new Function(newIndex, Functions[i]!.Value.Value, Functions[i]!.Value.Priority);
            }
        }
    }
}