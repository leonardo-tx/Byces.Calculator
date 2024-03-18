using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Byces.Calculator.Cache.Variables;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Collections
{
    internal sealed class Content
    {
        internal List<Variable> Variables { get; } = new();

        internal List<Operation> Operations { get; } = new();

        internal List<Function> Functions { get; } = new();

        internal void Clear()
        {
            Variables.Clear();
            Operations.Clear();
            Functions.Clear();
        }

        internal void CopyTo(CachedContent cachedContent)
        {
            cachedContent.Functions.AddRange(Functions);
            cachedContent.Operations.AddRange(Operations);
            for (int i = 0; i < Variables.Count; i++)
            {
                Variable currentVariable = Variables[i];
                if (currentVariable.VariableItem != null)
                {
                    if (currentVariable.Type == VariableType.Number)
                    {
                        cachedContent.Variables.Add(new CachedNumberItem(currentVariable.VariableItem, currentVariable.Boolean));
                        continue;
                    }
                    cachedContent.Variables.Add(new CachedItem(currentVariable.VariableItem));
                    continue;
                }
                if (currentVariable.Type == VariableType.Number)
                {
                    cachedContent.Variables.Add(new CachedNumber(currentVariable.Number));
                    continue;
                }
                cachedContent.Variables.Add(new CachedBoolean(currentVariable.Boolean));
            }
        }

        internal void Process(int priority = 0, int initialIndex = 0)
        {
            for (int i = initialIndex, lastPriority = priority; i < Operations.Count; i++)
            {
                Operation currentOperation = Operations[i];
                if (lastPriority >= currentOperation.Priority)
                {
                    lastPriority = currentOperation.Priority;
                    continue;
                }
                Process(currentOperation.Priority, i);
                
                i = initialIndex - 1;
                lastPriority = priority;
            }
            int lastIndex = Operations.Count - 1;
            OperatorPriority usedOperators = OperatorPriority.None;
            
            for (int i = initialIndex; i < Operations.Count; i++)
            {
                if (Operations[i].Priority != priority)
                {
                    lastIndex = i - 1;
                    break;
                }
                usedOperators |= Operations[i].Value.Priority;
            }
            CalculateInOrder(priority, initialIndex, lastIndex, usedOperators);
        }
        
        private void CalculateInOrder(int priority, int initialIndex, int lastIndex, OperatorPriority usedOperators)
        {
            CalculateFunctions(priority, initialIndex, lastIndex);
            
            if ((usedOperators & OperatorPriority.Potentiality) != 0) 
                CalculateOperations(OperatorPriority.Potentiality, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.Multiplicative) != 0) 
                CalculateOperations(OperatorPriority.Multiplicative, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.Additive) != 0) 
                CalculateOperations(OperatorPriority.Additive, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.Relational) != 0) 
                CalculateOperations(OperatorPriority.Relational, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.Equality) != 0) 
                CalculateOperations(OperatorPriority.Equality, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.AndBitwise) != 0) 
                CalculateOperations(OperatorPriority.AndBitwise, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.OrBitwise) != 0) 
                CalculateOperations(OperatorPriority.OrBitwise, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.AndConditional) != 0) 
                CalculateOperations(OperatorPriority.AndConditional, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.OrConditional) != 0) 
                CalculateOperations(OperatorPriority.OrConditional, initialIndex, ref lastIndex);
            if ((usedOperators & OperatorPriority.FunctionSeparator) != 0) 
                CalculateMultipleArgsFunction(initialIndex, lastIndex - initialIndex + 1);
        }

        private void CalculateFunctions(int priority, int initialIndex, int lastIndex)
        {
            int maxPriority = Functions.MaxPriority();
            if (maxPriority > priority) CalculateFunctions(maxPriority, initialIndex, lastIndex);
            
            for (int i = Functions.Count - 1; i >= 0; i--)
            {
                Function function = Functions[i];
                if (function.Priority != priority || function.VariableIndex < initialIndex || function.VariableIndex > lastIndex + 1) continue;
                
                int numberIndex = function.VariableIndex;

                Variables[numberIndex] = function.Operate(CollectionsMarshal.AsSpan(Variables).Slice(numberIndex, 1));
                Functions.RemoveAt(i);
            }
        }
        
        private void CalculateOperations(OperatorPriority operationPriority, int initialIndex, ref int lastIndex)
        {
            for (int i = initialIndex; i <= lastIndex; i++)
            {
                Operation operation = Operations[i];
                if (operationPriority != operation.Value.Priority) continue;

                Variables[i] = operation.Value.Operate(Variables[i], Variables[i + 1]);
                Operations.RemoveAt(i);
                Variables.RemoveAt(i + 1);

                ReduceNumberIndexToFunctions(i + 1, 1);
                i--;
                lastIndex--;
            }
        }

        private void CalculateMultipleArgsFunction(int firstIndex, int count)
        {
            int functionIndex = FindFunctionIndex(firstIndex);
            if (functionIndex == -1) throw new MissingFunctionExpressionException();
            
            ReadOnlySpan<Variable> values = CollectionsMarshal.AsSpan(Variables).Slice(firstIndex, count + 1);
            
            Variables[firstIndex] = Functions[functionIndex].Operate(values);
            Operations.RemoveRange(firstIndex, count);
            Variables.RemoveRange(firstIndex + 1, count);
            Functions.RemoveAt(functionIndex);

            ReduceNumberIndexToFunctions(firstIndex + 1, count);
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