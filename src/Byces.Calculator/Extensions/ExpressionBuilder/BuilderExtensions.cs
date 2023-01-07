using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using MathNet.Numerics;
using System;
using System.Collections.Generic;

namespace Byces.Calculator.Extensions.ExpressionBuilder
{
    internal static class BuilderExtensions
    {
        internal static readonly string[] AllSelfOperationsBefore = new string[] { "√", "SQRT" };
        internal const string AllSelfOperationsAfter = "!";
        internal const string AllOperations = "+-*/^√";
        internal const string AllNumberComplements = "+-.,πe";

        internal static ReadOnlySpan<char> GetValidNumberSlice(this ReadOnlySpan<char> source)
        {
            int firstIndex = 0, count = source.Length;
            bool isAfterNumber = false;
            for (int i = 0; i < source.Length; i++)
            {
                if (!char.IsDigit(source[i]) && !AllNumberComplements.Contains(source[i]))
                {
                    if (isAfterNumber) { count--; continue; }
                    firstIndex++; continue;
                }
                isAfterNumber = true;
            }
            return source[firstIndex..count];
        }

        internal static int Count(this ReadOnlySpan<char> source, ReadOnlySpan<char> characters)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (source[i] == characters[j]) count++;
                }
            }
            return count;
        }

        internal static int CountOperations(this ReadOnlySpan<char> source)
        {
            ReadOnlySpan<char> operations = AllOperations;
            int count = 0;

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '√')
                {
                    for (int j = 0; j < operations.Length; j++)
                    {
                        if (i != 0 && source[i - 1] == operations[j]) continue;
                        count++; break;
                    }
                    continue;
                }
                for (int j = 0; j < operations.Length; j++)
                {
                    if (source[i] == operations[j]) count++;
                }
            }
            return count;
        }

        internal static int CountSelfOperations(this ReadOnlySpan<char> source)
        {
            ReadOnlySpan<string> beforeOperations = AllSelfOperationsBefore;
            int count = source.Count(AllSelfOperationsAfter);

            for (int i = 0, firstIndex = 0; i < source.Length; i++)
            {
                if (char.IsDigit(source[i]) || AllNumberComplements.Contains(source[i]))
                {
                    if (beforeOperations.CustomContains(source[firstIndex..i])) count++;
                    break;
                }
                if (beforeOperations.CustomContains(source[firstIndex..(i + 1)])) { count++; firstIndex = i + 1; }
                if (source[i] == '(') firstIndex = i + 1;

            }
            return count;
        }

        internal static bool AnyOperationAt(this ReadOnlySpan<char> source, int index)
        {
            return AllOperations.Contains(source[index]);
        }

        internal static double? TryGetAlternativeNumber(this ReadOnlySpan<char> source)
        {
            if (source.Length > 2) return null;

            int validNumberChar = (source.Length == 1) ? source[0] : source[1];
            double? result = validNumberChar switch
            {
                'π' => Constants.Pi,
                'e' => Constants.E,
                _ => null
            };
            if (result == null) return null;
            if (source.Length == 2)
            {
                switch (source[0])
                {
                    case '+': break;
                    case '-': result *= -1; break;
                    default: throw new ArgumentException("Provided expression has unknown symbols.");
                }
            }
            return result;
        }

        internal static IList<Operation> GetAllSelfOperations(this ReadOnlySpan<char> source, int lastPriority)
        {
            IList<Operation> selfOperations = Array.Empty<Operation>();
            int selfOperatorsCount = source.CountSelfOperations();

            if (selfOperatorsCount == 0) return selfOperations;

            ReadOnlySpan<string> allSelfOperatorsBeforeAsSpan = AllSelfOperationsBefore;
            selfOperations = new List<Operation>(selfOperatorsCount);
            int currentPriority = lastPriority + source.Count("(") - source.Count(")");
            bool isAfterNumber = true;
            for (int i = source.Length - 1, lastIndex = i; i >= 0; i--)
            {
                if (isAfterNumber && !char.IsDigit(source[i]) && !AllNumberComplements.Contains(source[i]))
                {
                    if (source[i] == ')') currentPriority++;
                    else selfOperations.Add(new Operation(source[i].GetSelfOperation(), currentPriority));
                    continue;
                }
                if (char.IsDigit(source[i]) || AllNumberComplements.Contains(source[i]))
                {
                    lastIndex = i - 1;
                    isAfterNumber = false;
                    continue;
                }
                if (source[i] == '(') { currentPriority--; lastIndex = i - 1; continue; }
                if (allSelfOperatorsBeforeAsSpan.CustomContains(source[i..(lastIndex + 1)]))
                {
                    if (lastIndex - i == 0) selfOperations.Add(new Operation(source[i].GetSelfOperation(), currentPriority));
                    else selfOperations.Add(new Operation(source[i..(lastIndex + 1)].GetSelfOperation(), currentPriority));
                    lastIndex = i - 1;
                }
            }
            return selfOperations;
        }

        private static bool CustomContains(this ReadOnlySpan<string> source, ReadOnlySpan<char> characters)
        {
            for (int i = 0; i < source.Length; i++)
            {
                ReadOnlySpan<char> current = source[i];
                if (current.Equals(characters, StringComparison.Ordinal)) return true;
            }
            return false;
        }

        internal static OperationType GetSelfOperation(this char operation)
        {
            return operation switch
            {
                '!' => OperationType.Factorial,
                '√' => OperationType.Root,
                _ => throw new NotSupportedException("Not supported operation.")
            };
        }

        internal static OperationType GetSelfOperation(this ReadOnlySpan<char> operation)
        {     
            if (MemoryExtensions.SequenceEqual(operation, MemoryExtensions.AsSpan("SQRT"))) return OperationType.Root;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static OperationType GetOperation(this char character)
        {
            return character switch
            {
                '√' => OperationType.Root,
                '^' => OperationType.Power,
                '*' => OperationType.Multiply,
                '/' => OperationType.Divide,
                '+' => OperationType.Add,
                '-' => OperationType.Subtract,
                _ => throw new NotSupportedException("Not supported operation.")
            };
        }
    }
}