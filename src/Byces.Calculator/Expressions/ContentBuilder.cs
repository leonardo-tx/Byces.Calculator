using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal ref struct ContentBuilder
    {
        internal ContentBuilder(ReadOnlySpan<char> expressionSpan)
        {
            ExpressionSpan = expressionSpan;
            AfterNumber = false;
            FirstIndex = 0;
            LastIndex = 0;
            Priority = 0;
            AddedNumbers = 0;
            AddedOperations = 0;
            AddedFunctions = 0;
            CurrentNumber = null;
        }
        private ReadOnlySpan<char> ExpressionSpan { get; }

        private int FirstIndex { get; set; }

        private int LastIndex { get; set; }

        private int Priority { get; set; }

        private bool AfterNumber { get; set; }

        private int AddedNumbers { get; set; }

        private int AddedOperations { get; set; }

        private int AddedFunctions { get; set; }

        private double? CurrentNumber { get; set; }

        private ReadOnlySpan<char> CurrentSpan => ExpressionSpan[FirstIndex..(LastIndex + 1)];

        private char CurrentChar => ExpressionSpan[LastIndex];

        private char NextChar => ExpressionSpan[LastIndex + 1];

        private bool IsLastIndex() => LastIndex + 1 == ExpressionSpan.Length;

        private void Reset()
        {
            FirstIndex = LastIndex;
            AfterNumber = false;
            CurrentNumber = null;
        }

        internal void Build(Content content, BuilderInfo builderInfo)
        {
            for (; LastIndex < ExpressionSpan.Length; LastIndex++, FirstIndex++)
            {
                if (builderInfo.HasPriority && FindParentheses()) continue;
                if (!AfterNumber)
                {
                    if (FindNumber() || (builderInfo.HasSpecialNumber && FindSpecialNumber())) { AfterNumber = true; continue; }
                    if (builderInfo.HasFunction && FindFunction(content)) { FirstIndex = LastIndex; continue; }
                }
                else
                {
                    if (FindOperation(content)) { Reset(); continue; }
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
            if (LastIndex != FirstIndex) return false;
            bool hasSignal = CurrentChar == '+' || CurrentChar == '-';
            
            if (hasSignal) LastIndex++;
            while (LastIndex < ExpressionSpan.Length && (char.IsDigit(CurrentChar) || CurrentChar == '.' || CurrentChar == ','))
            {
                LastIndex++;
                if (LastIndex == ExpressionSpan.Length) break;
                if (CurrentChar == 'E' || CurrentChar == 'e')
                {
                    if (IsLastIndex()) throw new UnknownNumberExpressionException();
                    if (NextChar != '+' && NextChar != '-') throw new UnknownNumberExpressionException();
                    
                    LastIndex += 2;
                }
            }
            if (LastIndex == FirstIndex) return false;
            if (LastIndex - FirstIndex == 1 && hasSignal) return false;

            CurrentNumber = double.Parse(ExpressionSpan[FirstIndex..(--LastIndex + 1)]);
            FirstIndex = LastIndex;
            return true;
        }

        private bool FindSpecialNumber()
        {
            if (LastIndex == ExpressionSpan.Length || !SpecialNumberType.TryParse(CurrentSpan, out double number)) return false;

            CurrentNumber = number;
            FirstIndex = LastIndex;

            return true;
        }

        private bool FindFunction(Content content)
        {
            if (LastIndex == ExpressionSpan.Length || !FunctionType.TryParse(CurrentSpan, out FunctionType functionType)) return false;
            if (functionType.AdditionalCheck > 0 && !IsLastIndex() && char.IsLetter(NextChar))
            {
                for (int i = 1; i <= functionType.AdditionalCheck; i++)
                {
                    if (!FunctionType.TryParse(ExpressionSpan[FirstIndex..(LastIndex + 1 + i)], out FunctionType functionType2)) continue;

                    LastIndex += i;
                    AddFunction(content, functionType2);
                    return true;
                }
            }
            AddFunction(content, functionType);
            return true;
        }

        private void AddFunction(Content content, FunctionType functionType)
        {
            Function function = new Function(AddedNumbers, functionType, Priority);
            content.Functions[AddedFunctions++] = function;
        }

        private bool FindOperation(Content content)
        {
            if (OperationType.TryParse(CurrentSpan, out var operationType))
            {
                AddNumber(content);
                AddOperation(content, operationType);
                return true;
            }
            return false;
        }

        private void AddNumber(Content content)
        {
            if (CurrentNumber == null) throw new IncompleteExpressionException();
            content.Numbers[AddedNumbers++] = CurrentNumber.Value;
        }

        private void AddOperation(Content content, OperationType operationType)
        {
            Operation operation = new Operation(operationType, Priority);
            content.Operations[AddedOperations++] = operation;
        }
    }
}