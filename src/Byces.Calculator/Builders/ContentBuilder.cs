using Byces.Calculator.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Expressions.Items;
using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Builders
{
    internal sealed class ContentBuilder
    {
        public ContentBuilder(Content content, CalculatorDependencies dependencies)
        {
            _content = content;
            _builtExpressions = dependencies.BuiltExpressions;
            _cultureInfo = dependencies.CultureInfo;
        }

        internal bool InconstantResult;

        private int _firstIndex;

        private int _lastIndex;

        private int _priority;

        private bool _isNegative;

        private char _groupSeparator;

        private char _decimalSeparator;

        private NumberFormatInfo _numberFormatInfo = null!;

        private readonly Content _content;
        
        private readonly BuiltExpressions _builtExpressions;

        private readonly CultureInfo? _cultureInfo;

        public void Clear()
        {
            _firstIndex = 0;
            _lastIndex = 0;
            _priority = 0;
            _isNegative = false;
            InconstantResult = false;
        }

        public void Build(ReadOnlySpan<char> expressionSpan)
        {
            CultureInfo currentCultureInfo = _cultureInfo ?? Thread.CurrentThread.CurrentCulture;
            
            _numberFormatInfo = currentCultureInfo.NumberFormat;
            _groupSeparator = currentCultureInfo.NumberFormat.NumberGroupSeparator[0];
            _decimalSeparator = currentCultureInfo.NumberFormat.NumberDecimalSeparator[0];
            ScanExpression(expressionSpan);
        }
        
        private void ScanExpression(ReadOnlySpan<char> expressionSpan)
        {
            bool reachedEnd = false;
            do
            {
                while (!reachedEnd && !ScanExpressionBeforeVariable(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
                while (!reachedEnd && !ScanExpressionAfterVariable(expressionSpan))
                {
                    IncreaseCycle(expressionSpan, out reachedEnd);
                }
                IncreaseCycle(expressionSpan, out reachedEnd);
            } while (!reachedEnd);
            
            if (_priority > 0) throw new MissingParenthesesExpressionException();
            if (_content.Variables.Count != _content.Operations.Count + 1) throw new IncompleteExpressionException();
            if (_lastIndex != _firstIndex) throw new UnknownNumberExpressionException();
        }
        
        private void IncreaseCycle(ReadOnlySpan<char> expressionSpan, out bool reachedEnd)
        {
            ++_lastIndex;
            ++_firstIndex;
            reachedEnd = _lastIndex >= expressionSpan.Length;
        }
        
        private bool ScanExpressionBeforeVariable(ReadOnlySpan<char> expressionSpan)
        {
            if (expressionSpan[_lastIndex] == '(')
            {
                ++_priority;
                return false;
            }
            if (FindNumber(expressionSpan))
            {
                _firstIndex = _lastIndex;
                return true;
            }
            while (!FindVariableOrFunction(expressionSpan))
            {
                ++_lastIndex;
            }
            _firstIndex = _lastIndex;
            return _content.Variables.Count == _content.Operations.Count + 1;
        }
        
        private bool ScanExpressionAfterVariable(ReadOnlySpan<char> expressionSpan)
        {
            if (expressionSpan[_lastIndex] == ')')
            {
                if (--_priority < 0) throw new MisplacedParenthesesExpressionException();
                return false;
            }
            if (expressionSpan[_lastIndex] == '(')
            {
                AddOperator(_builtExpressions.AfterConflictItems.Parse("*"));
                ++_priority;
                return true;
            }
            while (!FindOperator(expressionSpan))
            {
                ++_lastIndex;
            }
            _firstIndex = _lastIndex;
            return true;
        }
        
        private bool FindNumber(ReadOnlySpan<char> expressionSpan)
        {
            _isNegative = expressionSpan[_lastIndex] == '-';
            if (_isNegative || expressionSpan[_lastIndex] == '+') { _lastIndex++; _firstIndex++; }
            if (char.IsLetter(expressionSpan[_lastIndex])) return false;
            
            var numberStyles = NumberStyles.None;
            for (bool hasSignal = false; _lastIndex < expressionSpan.Length; _lastIndex++)
            {
                char currentChar = expressionSpan[_lastIndex];
                if (char.IsDigit(currentChar)) continue;
                if ((numberStyles & NumberStyles.AllowDecimalPoint) == NumberStyles.None)
                {
                    if (currentChar == _decimalSeparator) { numberStyles |= NumberStyles.AllowDecimalPoint; continue; }
                    if (currentChar == _groupSeparator) { numberStyles |= NumberStyles.AllowThousands; continue; }
                }
                if ((numberStyles & NumberStyles.AllowExponent) == 0)
                {
                    if (currentChar is 'E' or 'e')
                    {
                        numberStyles |= NumberStyles.AllowExponent; continue;
                    }
                }
                else if (!hasSignal && currentChar is '+' or '-')
                {
                    hasSignal = true; continue;
                }
                break;
            }
            return ParseNumber(expressionSpan[_firstIndex.._lastIndex], numberStyles);
        }
        
        private bool ParseNumber(ReadOnlySpan<char> currentSpan, NumberStyles numberStyles)
        {
            if (currentSpan.IsEmpty) return false;
            _lastIndex--;

            if (!double.TryParse(currentSpan, numberStyles, _numberFormatInfo, out double result)) throw new UnknownNumberExpressionException();

            AddNumber(result);
            return true;
        }
        
        private bool FindOperator(ReadOnlySpan<char> expressionSpan)
        {
            if (_lastIndex == expressionSpan.Length) return true;
            if (!FindExpression(expressionSpan, _builtExpressions.AfterConflictItems, out OperatorItem? result)) return false;
            
            AddOperator(result);
            return true;
        }

        private bool FindVariableOrFunction(ReadOnlySpan<char> expressionSpan)
        {
            if (_lastIndex == expressionSpan.Length) return true;
            if (!FindExpression(expressionSpan, _builtExpressions.BeforeConflictItems, out BeforeVariableItem? result)) return false;
            if (!result.Pure)
            {
                InconstantResult = true;
            }
            if (result is VariableItem variableRepresentation2)
            {
                AddVariable(variableRepresentation2);
                return true;
            }
            AddFunction((FunctionItem)result);
            return true;
        }

        private bool FindExpression<T>(ReadOnlySpan<char> expressionSpan, ConflictItems<T> conflictItems, [NotNullWhen(true)] out T? result) 
            where T : ExpressionItem<T>
        {
            ReadOnlySpan<char> currentSpan = expressionSpan[_firstIndex..(_lastIndex + 1)];
            if (!conflictItems.TryParse(currentSpan, out result)) return false;

            int increaseLastIndex = 0;
            T? value = null;
            
            for (int i = 0; i < result.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = result.RepresentableConflicts[i];

                int j = representableConflict.Difference;
                if (_lastIndex + j >= expressionSpan.Length) continue;
                
                currentSpan = expressionSpan[_firstIndex..(_lastIndex + j + 1)];
                
                bool parseResult = conflictItems.TryParse(currentSpan, out T? value2);
                if (!parseResult || increaseLastIndex > j) continue;
                
                increaseLastIndex = j;
                value = value2;
            }
            if (value != null)
            {
                result = value;
                _lastIndex += increaseLastIndex;
            }
            return true;
        }
        
        private void AddFunction(FunctionItem item)
        {
            Function function = new(_content.Variables.Count, item, _priority);
            _content.Functions.Add(function);
        }
        
        private void AddVariable(VariableItem item)
        {
            if (!item.Pure)
            {
                Variable value = new(item, _isNegative);
                _content.Variables.Add(value);

                return;
            }
            if (item.VariableType == VariableType.Number)
            {
                NumberItem numberItem = (NumberItem)item;
                AddNumber(numberItem.GetValue());
                    
                return;
            }
            _content.Variables.Add(item.GetVariable());
        }
        
        private void AddNumber(double givenValue)
        {
            _content.Variables.Add(_isNegative ? -givenValue : givenValue);
        }
        
        private void AddOperator(OperatorItem item)
        {
            Operation operation = new(item, _priority);
            _content.Operations.Add(operation);
        }
    }
}