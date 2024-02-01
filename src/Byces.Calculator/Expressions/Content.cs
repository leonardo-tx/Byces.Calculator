using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Byces.Calculator.Expressions
{
    internal sealed class Content
    {
        private static readonly OperatorPriority[] OperatorPriorities = Enum
            .GetValues(typeof(OperatorPriority))
            .Cast<OperatorPriority>().ToArray();

        internal List<Variable> Variables { get; } = new();

        internal List<Operation> Operations { get; } = new();

        internal List<Function> Functions { get; } = new();

        internal OperatorPriority UsedOperators { get; set; } = OperatorPriority.None;

        internal void Clear()
        {
            UsedOperators = OperatorPriority.None;
            Variables.Clear();
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
            for (int i = 0; i < OperatorPriorities.Length - 1; i++)
            {
                if ((UsedOperators & OperatorPriorities[i]) == OperatorPriority.None) continue;
                CalculateOperations(OperatorPriorities[i], priority);
            }
            if ((UsedOperators & OperatorPriority.SemiColon) == OperatorPriority.SemiColon) FindSemiColon(priority);
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
                int numberIndex = Functions[i].VariableIndex;

                Variables[numberIndex] = Functions[i].Operate(Variables[numberIndex]);
                Functions.RemoveAt(i);
            }
        }
        
        private void CalculateOperations(OperatorPriority operationPriority, int priority)
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                if (operation.Priority != priority) continue;
                if (operationPriority != operation.Value.Priority) continue;

                Variables[i] = operation.Value.Operate(Variables[i], Variables[i + 1]);
                Operations.RemoveAt(i);
                Variables.RemoveAt(i + 1);

                ReduceNumberIndexToFunctions(i + 1, 1);
                i--;
            }
        }
        
        private void FindSemiColon(int priority)
        {
            int firstIndex = -1;
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                if (firstIndex != -1 && (operation.Priority != priority || operation.Value.Priority != OperatorPriority.SemiColon))
                {
                    int count = i - firstIndex + 1;
                    CalculateMultipleArgsFunction(firstIndex, count);

                    firstIndex = -1; i -= count; continue;
                }
                if (firstIndex == -1 && operation.Priority == priority && operation.Value.Priority == OperatorPriority.SemiColon)
                {
                    firstIndex = i;
                }
            }
            if (firstIndex == -1) return;
            CalculateMultipleArgsFunction(firstIndex, Variables.Count - firstIndex);
        }

        private void CalculateMultipleArgsFunction(int firstIndex, int count)
        {
            int functionIndex = FindFunctionIndex(firstIndex);
            if (functionIndex == -1) throw new MissingFunctionExpressionException();
            
            ReadOnlySpan<Variable> values = CollectionsMarshal.AsSpan(Variables).Slice(firstIndex, count);
            
            Variables[firstIndex] = Functions[functionIndex].Operate(values);
            Operations.RemoveRange(firstIndex, count - 1);
            Variables.RemoveRange(firstIndex + 1, count - 1);
            Functions.RemoveAt(functionIndex);

            ReduceNumberIndexToFunctions(firstIndex + 1, count - 1);
        }
        
        private int FindFunctionIndex(int variableIndex)
        {
            ReadOnlySpan<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (span[i].VariableIndex == variableIndex) return i;
            }
            return -1;
        }

        private void ReduceNumberIndexToFunctions(int initialVariableIndex, int removedCount)
        {
            Span<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                Function function = span[i];

                if (function.VariableIndex < initialVariableIndex) break;
                span[i] = new Function(function.VariableIndex - removedCount, function.Value, function.Priority);
            }
        }
    }
}