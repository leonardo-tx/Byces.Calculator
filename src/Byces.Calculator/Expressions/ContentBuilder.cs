using Byces.Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Byces.Calculator.Expressions
{
    internal ref struct ContentBuilder
    {
        internal ContentBuilder(ReadOnlySpan<char> expressionSpan)
        {
            ExpressionSpan = expressionSpan;
            IsNumber = false;
            FirstIndex = 0; LastIndex = 0; Priority = 0; NumberCount = 0; SelfOperationsCount = 0;
            NumberRange = 0..0; BeforeRange = 0..0; AfterRange = 0..0;
        }
        private ReadOnlySpan<char> ExpressionSpan { get; }

        private Range NumberRange { get; set; }

        private Range BeforeRange { get; set; }

        private Range AfterRange { get; set; }

        private int NumberCount { get; set; }

        private int SelfOperationsCount { get; set; }

        private int FirstIndex { get; set; }

        private int LastIndex { get; set; }

        private int Priority { get; set; }

        private bool IsNumber { get; set; }

        private ReadOnlySpan<char> CurrentSpan => ExpressionSpan[FirstIndex..(LastIndex + 1)];

        private ReadOnlySpan<char> NextSpan => ExpressionSpan[FirstIndex..(LastIndex + 2)];

        private ReadOnlySpan<char> CurrentNumber => ExpressionSpan[NumberRange];

        private ReadOnlySpan<char> CurrentBefore => ExpressionSpan[BeforeRange];

        private ReadOnlySpan<char> CurrentAfter => ExpressionSpan[AfterRange];

        private char CurrentChar => ExpressionSpan[LastIndex];

        private char NextChar => ExpressionSpan[LastIndex + 1];

        private bool IsLastIndex() => LastIndex + 1 == ExpressionSpan.Length;

        private void Reset()
        {
            NumberRange = 0..0; BeforeRange = 0..0; AfterRange = 0..0;
            FirstIndex = LastIndex;
            NumberCount = 0; SelfOperationsCount = 0;
            IsNumber = false;
        }

        internal void Build(Content content)
        {
            for (; LastIndex < ExpressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindParentheses()) continue;
                if (FindNumber()) { IsNumber = true; continue; }
                if (FindOperation(content)) continue;
                if (FindBeforeSelfOperation() || FindAfterSelfOperation())
                {
                    SelfOperationsCount++;
                    FirstIndex = LastIndex; continue;
                }
                FirstIndex--;
            }
            AddNumber(content);
        }

        private bool FindParentheses()
        {
            if (CurrentChar == '(')
            {
                Priority++; BeforeRange = GetCurrentRange(BeforeRange);
                return true;
            }
            if (CurrentChar == ')')
            {
                Priority--; AfterRange = GetCurrentRange(AfterRange);
                return true;
            }
            return false;
        }

        private bool FindNumber()
        {
            if (!IsNumber && (CurrentChar == '+' || CurrentChar == '-'))
            {
                if (char.IsDigit(NextChar)) { NumberCount++; return true; }

                FirstIndex--;
                return true;
            }
            if (CurrentChar == '.' || CurrentChar == ',' || char.IsDigit(CurrentChar) || Number.TryParse(CurrentSpan, out _))
            {
                if (LastIndex - FirstIndex > 0)
                {
                    NumberRange = FirstIndex..(LastIndex + 1);
                    FirstIndex = LastIndex;
                }
                else
                {
                    NumberRange = (FirstIndex - NumberCount)..(LastIndex + 1);
                    NumberCount++;
                }
                return true;
            }
            return false;
        }

        private bool FindOperation(Content content)
        {
            if (IsNumber && OperationType.TryParse(CurrentSpan, OperationCategory.None, out var operationType))
            {
                AddNumber(content);
                AddOperation(content, operationType);
                Reset();

                return true;
            }
            return false;
        }

        private void AddNumber(Content content)
        {
            IList<Operation> selfOperations = GetAllSelfOperations(content.Operations.LastOrDefault().Priority);
            Number number = Number.Parse(CurrentNumber, selfOperations);

            content.Numbers.Add(number);
        }

        private IList<Operation> GetAllSelfOperations(int priority)
        {
            if (SelfOperationsCount == 0) return Array.Empty<Operation>();

            IList<Operation> selfOperations = new List<Operation>(SelfOperationsCount);
            ReadOnlySpan<char> beforeOperations = CurrentBefore;
            ReadOnlySpan<char> afterOperations = CurrentAfter;

            for (int i = 0, firstIndex = 0; i < beforeOperations.Length; i++)
            {
                if (beforeOperations[i] == '(') { priority++; firstIndex = i + 1; continue; }
                if (OperationType.TryParse(beforeOperations[firstIndex..(i + 1)], OperationCategory.Before, out OperationType operationType))
                {
                    selfOperations.Add(new Operation(operationType, priority));
                    firstIndex = i + 1;
                }
            }
            for (int i = 0, firstIndex = 0; i < afterOperations.Length; i++)
            {
                if (afterOperations[i] == ')') { priority--; firstIndex = i + 1; continue; }
                if (OperationType.TryParse(afterOperations[firstIndex..(i + 1)], OperationCategory.After, out OperationType operationType))
                {
                    selfOperations.Add(new Operation(operationType, priority));
                    continue;
                }
            }
            return selfOperations;
        }

        private void AddOperation(Content content, OperationType operationType)
        {
            Operation operation = new Operation(operationType, Priority);
            content.Operations.Add(operation);
        }

        private bool FindBeforeSelfOperation()
        {
            if (!IsNumber && OperationType.TryParse(CurrentSpan, OperationCategory.Before, out _))
            {
                if (!IsLastIndex() && char.IsLetter(NextChar) &&
                    OperationType.TryParse(NextSpan, OperationCategory.Before, out _))
                {
                    LastIndex++;
                }
                BeforeRange = GetCurrentRange(BeforeRange);
                return true;
            }
            return false;
        }

        private bool FindAfterSelfOperation()
        {
            if (IsNumber && OperationType.TryParse(CurrentSpan, OperationCategory.After, out _))
            {
                AfterRange = GetCurrentRange(AfterRange);
                return true;
            }
            return false;
        }

        private Range GetCurrentRange(Range range)
        {
            if (range.Start.Value == range.End.Value)
            {
                return FirstIndex..(LastIndex + 1);
            }
            return range.Start.Value..(LastIndex + 1);
        }
    }
}