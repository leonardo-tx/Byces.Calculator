using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions
{
    internal ref struct ContentBuilder
    {
        internal ContentBuilder(ReadOnlySpan<char> expressionSpan)
        {
            ExpressionSpan = expressionSpan;
            IsNumber = false;
            FirstIndex = 0; LastIndex = 0; Priority = 0; NumberCount = 0; AddedNumbers= 0; AddedOperations = 0; AddedSelfOperations = 0;
            NumberRange = 0..0;
        }
        private ReadOnlySpan<char> ExpressionSpan { get; }

        private Range NumberRange { get; set; }

        private int NumberCount { get; set; }

        private int FirstIndex { get; set; }

        private int LastIndex { get; set; }

        private int Priority { get; set; }

        private bool IsNumber { get; set; }

        private int AddedNumbers { get; set; }

        private int AddedOperations { get; set; }

        private int AddedSelfOperations { get; set; }

        private ReadOnlySpan<char> CurrentSpan => ExpressionSpan[FirstIndex..(LastIndex + 1)];

        private ReadOnlySpan<char> NextSpan => ExpressionSpan[FirstIndex..(LastIndex + 2)];

        private ReadOnlySpan<char> CurrentNumber => ExpressionSpan[NumberRange];

        private char CurrentChar => ExpressionSpan[LastIndex];

        private char NextChar => ExpressionSpan[LastIndex + 1];

        private bool IsLastIndex() => LastIndex + 1 == ExpressionSpan.Length;

        private void Reset()
        {
            NumberRange = 0..0;
            FirstIndex = LastIndex;
            NumberCount = 0;
            IsNumber = false;
        }

        internal void Build(Content content)
        {
            for (; LastIndex < ExpressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindParentheses()) continue;
                if (FindNumber()) { IsNumber = true; continue; }
                if (FindOperation(content)) continue;
                if (FindBeforeSelfOperation(content))
                {
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
                Priority++;
                return true;
            }
            if (CurrentChar == ')')
            {
                Priority--;
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
            Number number = Number.Parse(CurrentNumber);
            content.Numbers[AddedNumbers++] = number;
        }

        private void AddOperation(Content content, OperationType operationType)
        {
            Operation operation = new Operation(operationType, Priority);
            content.Operations[AddedOperations++] = operation;
        }

        private bool FindBeforeSelfOperation(Content content)
        {
            if (!IsNumber && OperationType.TryParse(CurrentSpan, OperationCategory.Before, out OperationType operationType))
            {
                if (!IsLastIndex() && char.IsLetter(NextChar) &&
                    OperationType.TryParse(NextSpan, OperationCategory.Before, out OperationType operationType2))
                {
                    LastIndex++;
                    AddSelfOperation(content, operationType2);
                }
                else
                {
                    AddSelfOperation(content, operationType);
                }
                return true;
            }
            return false;
        }

        private void AddSelfOperation(Content content, OperationType operationType)
        {
            SelfOperation selfOperation = new SelfOperation(new Operation(operationType, Priority), AddedNumbers);
            content.SelfOperations[AddedSelfOperations++] = selfOperation;
        }
    }
}