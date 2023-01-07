using System;
using System.Collections.Generic;
using System.Linq;
using Byces.Calculator.Expressions;
using Byces.Calculator.Extensions.ExpressionBuilder;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="MathExpression"/> in a simplified way.
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
        /// Builds the <see cref="MathExpression"/> to be calculated.
        /// </summary>
        /// <returns>The built expression.</returns>
        public MathExpression Build()
        {
            if (string.IsNullOrWhiteSpace(Expression)) return MathExpression.Default;
            try
            {
                return new MathExpression(FormatExpression(), true);
            }
            catch (Exception ex)
            {
                return new MathExpression(ex.Message);
            }
        }

        private Content FormatExpression()
        {
            ReadOnlySpan<char> expressionSpan = Expression.Replace(" ", "");
            CheckParentheses(expressionSpan);

            int capacity = expressionSpan.CountOperations();
            IList<Operation> operations;
            IList<Number> numbers = new List<Number>(capacity + 1);

            if (capacity == 0) operations = Array.Empty<Operation>();
            else operations = new List<Operation>(capacity);

            var content = new Content(numbers, operations);
            ConfigureContent(expressionSpan, content);
            if ((content.Numbers.Count + content.Operations.Count) % 2 == 0) throw new ArgumentException("The provided expression is not complete.");

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
            if (unclosedParentheses > 0) throw new ArgumentException("The provided expression is missing parentheses to be closed.");
        }

        private static void ConfigureContent(ReadOnlySpan<char> expressionSpan, Content content)
        {
            for (int i = 0, numberFirstIndex = 0, priority = 0; i < expressionSpan.Length; i++)
            {
                if (content.Numbers.Count != content.Operations.Count)
                {
                    content.Operations.Add(new Operation(expressionSpan[i].GetOperation(), priority));

                    numberFirstIndex = i + 1;
                    continue;
                }
                switch (expressionSpan[i])
                {
                    case '-': case '+': continue;
                    case '(': priority++; continue;
                    case ')': priority--; break;
                }
                if (expressionSpan.Length == i + 1 || expressionSpan.AnyOperationAt(i + 1))
                {
                    AddNumber(expressionSpan[numberFirstIndex..(i + 1)], content);
                }
            }
        }

        private static void AddNumber(ReadOnlySpan<char> numberSlice, Content content)
        {
            ReadOnlySpan<char> validNumberSlice = numberSlice.GetValidNumberSlice();
            IList<Operation> selfOperations = numberSlice.GetAllSelfOperations(content.Operations.LastOrDefault().Priority);

            if (!double.TryParse(validNumberSlice, out double doubleNumber))
            {
                doubleNumber = validNumberSlice.TryGetAlternativeNumber() ?? throw new ArgumentException("Provided expression has unknown symbols.");
            }
            Number number = new Number(doubleNumber, selfOperations);
            content.Numbers.Add(number);
        }
    }
}