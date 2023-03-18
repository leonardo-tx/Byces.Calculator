using Byces.Calculator.Enums;
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
        private const int StackAllocationLimit = 256;

        private static readonly OperatorPriority[] _operatorPriorities = Enum.GetValues(typeof(OperatorPriority))
            .Cast<OperatorPriority>().ToArray();

        internal List<Variable> Variables { get; } = new List<Variable>();

        internal List<Operation> Operations { get; } = new List<Operation>();

        internal List<Function> Functions { get; } = new List<Function>();

        internal OperatorPriority UsedOperators { get; set; }

        internal void Build(ReadOnlySpan<char> expressionSpan)
        {
            ContentBuilder builder = new ContentBuilder();
            builder.Build(this, expressionSpan);
        }

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
            for (int i = 0; i < _operatorPriorities.Length - 1; i++)
            {
                if ((UsedOperators & _operatorPriorities[i]) == OperatorPriority.None) continue;
                CalculateOperations(_operatorPriorities[i], priority);
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

                OperatorRepresentation operationRepresentation = (OperatorRepresentation)operation.Value;
                if (operationPriority != operationRepresentation.Priority) continue;

                Variables[i] = operationRepresentation.Operate(Variables[i], Variables[i + 1]);
                Operations.RemoveAt(i);
                Variables.RemoveAt(i + 1);

                ReduceNumerIndexToFunctions(i + 1, 1);
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
                    CalculateMultipleArgsFunction((int)firstIndex!, count);

                    lastIndex = null; firstIndex = null; i -= count; continue;
                }
                if (!firstIndex.HasValue && operation.Priority == priority && operationPriorityType == OperatorPriority.SemiColon) { firstIndex = i; continue; }
            }
            if (!firstIndex.HasValue) return;
            CalculateMultipleArgsFunction((int)firstIndex!, lastIndex.HasValue ? (int)lastIndex - (int)firstIndex + 1 : Variables.Count - (int)firstIndex);
        }

        private void CalculateMultipleArgsFunction(int firstIndex, int count)
        {
            int functionIndex = FindFunctionIndex(firstIndex);
            if (functionIndex == -1) throw new MissingFunctionExpressionException();
#if NET5_0_OR_GREATER
            ReadOnlySpan<Variable> values = CollectionsMarshal.AsSpan(Variables).Slice(firstIndex, count);
#else
            Span<Variable> values = StackAllocationLimit >= count ? stackalloc Variable[count] : new Variable[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = Variables[firstIndex + i];
            }
#endif
            Variables[firstIndex] = Functions[functionIndex].Operate(values);
            Operations.RemoveRange(firstIndex, count - 1);
            Variables.RemoveRange(firstIndex + 1, count - 1);
            Functions.RemoveAt(functionIndex);

            ReduceNumerIndexToFunctions(firstIndex + 1, count - 1);
        }

#if NET5_0_OR_GREATER
        private int FindFunctionIndex(int variableIndex)
        {
            ReadOnlySpan<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (span[i].VariableIndex == variableIndex) return i;
            }
            return -1;
        }

        private void ReduceNumerIndexToFunctions(int initialVariableIndex, int removedCount)
        {
            Span<Function> span = CollectionsMarshal.AsSpan(Functions);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                Function function = span[i];

                if (function.VariableIndex < initialVariableIndex) break;
                span[i] = new Function(function.VariableIndex - removedCount, function.Value, function.Priority);
            }
        }
#else
        private int FindFunctionIndex(int variableIndex)
        {
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                if (Functions[i].VariableIndex == variableIndex) return i;
            }
            return -1;
        }

        private void ReduceNumerIndexToFunctions(int initialVariableIndex, int removedCount)
        {
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                Function function = Functions[i];

                if (function.VariableIndex < initialVariableIndex) break;
                Functions[i] = new Function(function.VariableIndex - removedCount, function.Value, function.Priority);
            }
        }
#endif
    }
}