using System;
using System.Collections.Generic;
using System.Linq;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Extensions;
using MathNet.Numerics;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="FormatedExpression"/> in a simplified way to be used in the <see cref="Calculator"/>.
    /// </summary>
    public sealed class ExpressionBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="ExpressionBuilder"/> class.
        /// </summary>
        public ExpressionBuilder()
        {
            Expression = string.Empty;
        }

        public const string AllSelfOperatorsBefore = "√";
        public const string AllSelfOperatorsAfter = "!";
        public const string AllOperations = "+-*/^√";
        public const string AllValidNumbers = "0123456789.,πe";

        /// <summary>
        /// Gets or sets the expression of a <see cref="ExpressionBuilder"/>.
        /// </summary>
        /// <returns>The stored expression of the builder, or <see cref="string.Empty"/> if none is set.</returns>
        public string Expression { get; set; }

        /// <summary>
        /// Sets the expression to be builded.
        /// </summary>
        public ExpressionBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="FormatedExpression"/> to be calculated.
        /// </summary>
        /// <returns>The built expression.</returns>
        public FormatedExpression Build()
        {
            if (string.IsNullOrWhiteSpace(Expression)) return FormatedExpression.Empty;
            try
            {
                return new FormatedExpression(FormatExpression(), true);
            }
            catch (Exception ex)
            {
                return new FormatedExpression(ex.Message);
            }
        }

        private Content FormatExpression()
        {
            ReadOnlySpan<char> expressionSpan = Expression.Replace(" ", "");
            CheckParentheses(expressionSpan);
            
            int capacity = expressionSpan.Count(AllOperations);
            IList<Operation> operations;
            IList<int> priorities;
            IList<Number> numbers = new List<Number>(capacity + 1);

            if (capacity == 0) 
            { 
                operations = Array.Empty<Operation>();
                priorities = Array.Empty<int>();
            }
            else 
            { 
                operations = new List<Operation>(capacity);
                priorities = new List<int>(capacity);
            }
            var content = new Content(numbers, operations, priorities);
            ConfigureContent(expressionSpan, content);
            if ((content.Numbers.Count + content.Operations.Count) % 2 == 0) throw new ArgumentException("The provided expression is not complete");

            return content;
        }

        private static void CheckParentheses(ReadOnlySpan<char> expressionSpan)
        {
            int unclosedParentheses = 0;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                switch (expressionSpan[i])
                {
                    case '(':
                        unclosedParentheses++;
                        break;
                    case ')':
                        unclosedParentheses--;
                        break;
                }
            }
            if (unclosedParentheses < 0) throw new ArgumentException("Provided expression has misplaced parentheses.");
            if (unclosedParentheses > 0) throw new ArgumentException("The provided expression is missing containers to be closed.");
        }

        private static void ConfigureContent(ReadOnlySpan<char> expressionSpan, Content content)
        {
            for (int i = 0, numberFirstIndex = 0, priority = 0; i < expressionSpan.Length; i++)
            {
                if (content.Numbers.Count != content.Operations.Count)
                {
                    content.Operations.Add(GetOperation(expressionSpan[i]));
                    content.Priorities.Add(priority);

                    numberFirstIndex = i + 1;
                    continue;
                }
                switch (expressionSpan[i])
                {
                    case '-': continue;
                    case '(': priority++; continue;
                    case ')': priority--; break;
                }
                if (expressionSpan.Length == i + 1 || AllOperations.Contains(expressionSpan[i + 1]))
                {
                    AddNumber(expressionSpan[numberFirstIndex..(i + 1)], content);
                }
            }
        }

        private static void AddNumber(ReadOnlySpan<char> numberSlice, Content content)
        {
            ReadOnlySpan<char> validNumberSlice = GetValidNumberSlice(numberSlice);
            IList<SelfOperation> selfOperations = Array.Empty<SelfOperation>();
            int selfOperatorsCount = numberSlice.Count(AllSelfOperatorsAfter + AllSelfOperatorsBefore);
            
            if (selfOperatorsCount > 0)
            {
                selfOperations = new List<SelfOperation>(selfOperatorsCount);
                int lastPriority = content.Priorities.LastOrDefault() + numberSlice.Count("(") - numberSlice.Count(")");
                for (int i = numberSlice.Length - 1; i >= 0 ; i--)
                {
                    if (AllSelfOperatorsAfter.Contains(numberSlice[i]) || AllSelfOperatorsBefore.Contains(numberSlice[i]))
                    {
                        selfOperations.Add(new SelfOperation(GetSelfOperation(numberSlice[i]), lastPriority));
                    }
                    else if (numberSlice[i] == ')') lastPriority++;
                    else if (numberSlice[i] == '(') lastPriority--;
                }
            }
            if (!double.TryParse(validNumberSlice, out double doubleNumber))
            {
                doubleNumber = validNumberSlice[0] switch
                {
                    'π' => Constants.Pi,
                    'e' => Constants.E,
                    _ => throw new ArgumentException("Provided expression has unknown symbols")
                };
            }
            Number number = new Number(doubleNumber, selfOperations);
            content.Numbers.Add(number);
        }

        private static ReadOnlySpan<char> GetValidNumberSlice(ReadOnlySpan<char> numberSlice)
        {
            int firstIndex = 0, count = numberSlice.Length;
            for (int i = 0; i < numberSlice.Length; i++)
            {
                if (numberSlice[i] == '(' || AllSelfOperatorsBefore.Contains(numberSlice[i])) firstIndex++;
                else if (numberSlice[i] == ')' || AllSelfOperatorsAfter.Contains(numberSlice[i])) count--;
            }
            return numberSlice[firstIndex..count];
        }

        private static Operation GetOperation(char character)
        {
            return character switch
            {
                '√' => Operation.Root,
                '^' => Operation.Power,
                '*' => Operation.Multiply,
                '/' => Operation.Divide,
                '+' => Operation.Add,
                '-' => Operation.Subtract,
                _ => throw new ArgumentException("Invalid operator.")
            };
        }

        private static Operation GetSelfOperation(char character)
        {
            return character switch
            {
                '!' => Operation.Factorial,
                '√' => Operation.Root,
                _ => throw new ArgumentException("Invalid self operator.")
            };
        }
    }
}