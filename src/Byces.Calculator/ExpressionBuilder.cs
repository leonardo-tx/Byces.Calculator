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
            return GetMathExpression(Expression);
        }

        /// <summary>
        /// Gets the <see cref="MathExpression"/> without having to create an <see cref="ExpressionBuilder"/> object.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>The built expression.</returns>
        public static MathExpression GetMathExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return MathExpression.Default;
            try
            {
                return new MathExpression(FormatExpression(expression), true);
            }
            catch (Exception ex)
            {
                return new MathExpression(ex.Message);
            }
        }

        private static Content FormatExpression(string expression)
        {
            ReadOnlySpan<char> expressionSpan = GetExpressionSpan(expression);
            CheckParentheses(expressionSpan);

            int capacity = expressionSpan.CountOperations();
            IList<Operation> operations;
            IList<Number> numbers = new List<Number>(capacity + 1);

            if (capacity == 0) operations = Array.Empty<Operation>();
            else operations = new List<Operation>(capacity);

            var content = new Content(numbers, operations);
            var contentBuilder = new ContentBuilder(expressionSpan);

            contentBuilder.Build(content);
            if ((content.Numbers.Count + content.Operations.Count) % 2 == 0) throw new ArgumentException("The provided expression is not complete.");

            return content;
        }

        private static ReadOnlySpan<char> GetExpressionSpan(string expression)
        {
            int spaceCharsCount = expression.AsSpan().Count(" ");
            if (spaceCharsCount == 0) return expression;

            ReadOnlySpan<char> reference = expression;
            Span<char> expressionSpan = stackalloc char[reference.Length - spaceCharsCount];
            for (int i = 0, j = 0; i < reference.Length; i++)
            {
                if (reference[i] == ' ') continue;
                expressionSpan[j++] = reference[i];
            }
            return expressionSpan.ToString();
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
    }
}