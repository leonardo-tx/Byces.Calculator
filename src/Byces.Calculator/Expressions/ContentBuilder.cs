using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal ref struct ContentBuilder
    {
        internal ContentBuilder(ReadOnlySpan<char> expressionSpan, BuilderInfo builderInfo)
        {
            HasSpecialNumber = builderInfo.HasSpecialNumber;
            ExpressionSpan = expressionSpan;
            AtNumber = false;
            IsSpecialNumber = true;
            FirstIndex = 0;
            LastIndex = 0;
            Priority = 0;
            NumberCount = 0;
            AddedNumbers = 0;
            AddedOperations = 0;
            AddedSelfOperations = 0;
            NumberRange = 0..0;
        }
        private ReadOnlySpan<char> ExpressionSpan { get; }

        private Range NumberRange { get; set; }

        private int NumberCount { get; set; }

        private int FirstIndex { get; set; }

        private int LastIndex { get; set; }

        private int Priority { get; set; }

        private bool AtNumber { get; set; }

        private bool IsSpecialNumber { get; set; }

        private bool HasSpecialNumber { get; }

        private int AddedNumbers { get; set; }

        private int AddedOperations { get; set; }

        private int AddedSelfOperations { get; set; }

        private ReadOnlySpan<char> CurrentSpan => ExpressionSpan[FirstIndex..(LastIndex + 1)];

        private ReadOnlySpan<char> CurrentNumber => ExpressionSpan[NumberRange];

        private char CurrentChar => ExpressionSpan[LastIndex];

        private char NextChar => ExpressionSpan[LastIndex + 1];

        private bool IsLastIndex() => LastIndex + 1 == ExpressionSpan.Length;

        private void Reset()
        {
            NumberRange = 0..0;
            FirstIndex = LastIndex;
            NumberCount = 0;
            AtNumber = false;
            IsSpecialNumber = true;
        }

        internal void Build(Content content)
        {
            for (; LastIndex < ExpressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (FindParentheses()) continue;
                if (FindNumber() || FindSpecialNumber()) { AtNumber = true; continue; }
                if (FindOperation(content)) continue;
                if (FindBeforeSelfOperation(content)) { FirstIndex = LastIndex; continue; }
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
            if (!AtNumber && (CurrentChar == '+' || CurrentChar == '-'))
            {
                if (IsLastIndex()) throw new UnknownNumberExpressionException();
                if (char.IsDigit(NextChar)) { NumberCount++; IsSpecialNumber = false; return true; }

                FirstIndex--;
                return true;
            }
            if (char.IsDigit(CurrentChar) || CurrentChar == '.' || CurrentChar == ',')
            {
                IsSpecialNumber = false;
                NumberRange = (FirstIndex - NumberCount)..(LastIndex + 1);
                NumberCount++;

                return true;
            }
            if (!IsSpecialNumber && AtNumber && (CurrentChar == 'E' || CurrentChar == 'e'))
            {
                if (IsLastIndex()) throw new UnknownNumberExpressionException();
                if (NextChar != '+' && NextChar != '-') throw new UnknownNumberExpressionException();

                NumberRange = (FirstIndex++ - NumberCount)..(++LastIndex + 1);
                NumberCount += 2;

                return true;
            }
            return false;
        }

        private bool FindSpecialNumber()
        {
            if (!HasSpecialNumber || !IsSpecialNumber || !SpecialNumberType.TryParse(CurrentSpan, out _)) return false;

            NumberRange = FirstIndex..(LastIndex + 1);
            FirstIndex = LastIndex;
            IsSpecialNumber = false;

            return true;
        }

        private bool FindOperation(Content content)
        {
            if (AtNumber && OperationType.TryParse(CurrentSpan, out var operationType))
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
            if (CurrentNumber.IsEmpty) throw new IncompleteExpressionException();

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
            if (AtNumber || !SelfOperationType.TryParse(CurrentSpan, out SelfOperationType selfOperationType)) return false;
            if (selfOperationType.AdditionalCheck > 0 && !IsLastIndex() && char.IsLetter(NextChar))
            {
                for (int i = 1; i <= selfOperationType.AdditionalCheck; i++)
                {
                    if (!SelfOperationType.TryParse(ExpressionSpan[FirstIndex..(LastIndex + 1 + i)], out SelfOperationType selfOperationType2)) continue;

                    LastIndex += i;
                    AddSelfOperation(content, selfOperationType2);
                    return true;
                }
            }
            AddSelfOperation(content, selfOperationType);
            return true;
        }

        private void AddSelfOperation(Content content, SelfOperationType selfOperationType)
        {
            SelfOperation selfOperation = new SelfOperation(AddedNumbers, selfOperationType, Priority);
            content.SelfOperations[AddedSelfOperations++] = selfOperation;
        }
    }
}