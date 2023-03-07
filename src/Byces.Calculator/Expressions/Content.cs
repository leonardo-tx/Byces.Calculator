using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Expressions
{
    internal sealed class Content
    {
        private const int StackAllocationLimit = 256;

        internal List<Value> Values { get; } = new List<Value>();

        internal List<Operation> Operations { get; } = new List<Operation>();

        internal List<Function> Functions { get; } = new List<Function>();

        internal ResultType ExpectedResult { get; set; }

        internal void Build(ReadOnlySpan<char> expressionSpan, ResultType expectedResult)
        {
            ContentBuilder builder = new ContentBuilder();
            builder.Build(this, expressionSpan);

            ExpectedResult = expectedResult;
        }

        internal void Clear()
        {
            Values.Clear();
            Operations.Clear();
            Functions.Clear();
        }

        internal void Process()
        {
            int priority = Operations.MaxPriority();
            while (priority >= 0)
            {
                CalculateInOrder(priority);
                int newPriority = Operations.MaxPriority();

                if (newPriority == priority) break;
                priority = newPriority;
            }
            CalculateFunctions(0);
        }

        private void CalculateInOrder(int priority)
        {
            CalculateFunctions(priority);
            int count = ExpectedResult == ResultType.Boolean ? (int)OperationPriorityType.SemiColon : (int)OperationPriorityType.Fourth;
            for (int i = (int)OperationPriorityType.First; i < count; i++)
            {
                CalculateOperations((OperationPriorityType)i, priority);
            }
            FindSemiColon(priority);
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

                Values[numberIndex] = Functions[i].Operate(Values[numberIndex]);
                Functions.RemoveAt(i);
            }
        }

        private void CalculateOperations(OperationPriorityType operationPriority, int priority)
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                if (operation.Priority != priority) continue;

                OperationType operationType = (OperationType)operation.Value;
                if (operationPriority != operationType.Priority) continue;

                Values[i] = operationType.Operate(Values[i], Values[i + 1]);
                Operations.RemoveAt(i);
                Values.RemoveAt(i + 1);

                ReduceNumerIndexToFunctions(i + 1, 1);
                i--;
            }
        }

        private void FindSemiColon(int priority)
        {
            int? firstIndex = null, lastIndex = null;
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                OperationPriorityType operationPriorityType = ((OperationType)operation.Value).Priority;
                if (firstIndex.HasValue && (operation.Priority != priority || operationPriorityType != OperationPriorityType.SemiColon))
                {
                    lastIndex = i;
                    int count = (int)lastIndex - (int)firstIndex + 1;
                    CalculateMultipleArgsFunction((int)firstIndex!, count);

                    lastIndex = null; firstIndex = null; i -= count; continue;
                }
                if (!firstIndex.HasValue && operation.Priority == priority && operationPriorityType == OperationPriorityType.SemiColon) { firstIndex = i; continue; }
            }
            if (!firstIndex.HasValue) return;
            CalculateMultipleArgsFunction((int)firstIndex!, lastIndex.HasValue ? (int)lastIndex - (int)firstIndex + 1 : Values.Count - (int)firstIndex);
        }

        private void CalculateMultipleArgsFunction(int firstIndex, int count)
        {
            int functionIndex = FindFunctionIndex(firstIndex);
            if (functionIndex == -1) throw new MissingFunctionExpressionException();
#if NET5_0_OR_GREATER
            ReadOnlySpan<Value> values = CollectionsMarshal.AsSpan(Values).Slice(firstIndex, count);
#else
            Span<Value> values = StackAllocationLimit >= count ? stackalloc Value[count] : new Value[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = Values[firstIndex + i];
            }
#endif
            Values[firstIndex] = Functions[functionIndex].Operate(values);
            Operations.RemoveRange(firstIndex, count - 1);
            Values.RemoveRange(firstIndex + 1, count - 1);
            Functions.RemoveAt(functionIndex);

            ReduceNumerIndexToFunctions(firstIndex + 1, count - 1);
        }

#if NET5_0_OR_GREATER
        internal int FindFunctionIndex(int numberIndex)
        {
            ReadOnlySpan<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (span[i].NumberIndex == numberIndex) return i;
            }
            return -1;
        }

        private void ReduceNumerIndexToFunctions(int initialNumberIndex, int removedCount)
        {
            Span<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                Function function = span[i];

                if (function.NumberIndex < initialNumberIndex) break;
                span[i] = new Function(function.NumberIndex - removedCount, function.Value, function.Priority);
            }
        }
#else
        internal int FindFunctionIndex(int numberIndex)
        {
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                if (Functions[i].NumberIndex == numberIndex) return i;
            }
            return -1;
        }

        private void ReduceNumerIndexToFunctions(int initialNumberIndex, int removedCount)
        {
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                Function function = Functions[i];

                if (function.NumberIndex < initialNumberIndex) break;
                Functions[i] = new Function(function.NumberIndex - removedCount, function.Value, function.Priority);
            }
        }
#endif
    }
}