﻿using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using Byces.Calculator.Representations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CalculateOperations(OperatorPriority operationPriority, int priority)
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                if (operation.Priority != priority) continue;

                var operationRepresentation = (OperatorRepresentation)operation.Value;
                if (operationPriority != operationRepresentation.Priority) continue;

                Variables[i] = operationRepresentation.Operate(Variables[i], Variables[i + 1]);
                Operations.RemoveAt(i);
                Variables.RemoveAt(i + 1);

                ReduceNumberIndexToFunctions(i + 1, 1);
                i--;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FindSemiColon(int priority)
        {
            int? firstIndex = null, lastIndex = null;
            for (int i = 0; i < Operations.Count; i++)
            {
                Operation operation = Operations[i];
                OperatorPriority operationPriorityType = ((OperatorRepresentation)operation.Value).Priority;
                if (firstIndex.HasValue && (operation.Priority != priority || operationPriorityType != OperatorPriority.SemiColon))
                {
                    lastIndex = i;
                    int count = (int)lastIndex - (int)firstIndex + 1;
                    CalculateMultipleArgsFunction((int)firstIndex, count);

                    lastIndex = null; firstIndex = null; i -= count; continue;
                }
                if (!firstIndex.HasValue && operation.Priority == priority && operationPriorityType == OperatorPriority.SemiColon)
                {
                    firstIndex = i;
                }
            }
            if (!firstIndex.HasValue) return;
            CalculateMultipleArgsFunction((int)firstIndex, lastIndex.HasValue ? (int)lastIndex - (int)firstIndex + 1 : Variables.Count - (int)firstIndex);
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