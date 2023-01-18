using System;
using System.Linq;
using Byces.Calculator.Expressions;
using Byces.Calculator.Extensions;

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
                return FormatExpression(expression);
            }
            catch (Exception ex)
            {
                return new MathExpression(ex.Message);
            }
        }

        private static MathExpression FormatExpression(string expression)
        {
            int spaceCharsCount = expression.AsSpan().Count(" ");
            if (spaceCharsCount == 0) return BuildMathExpression(expression);

            Span<char> expressionSpan = stackalloc char[expression.Length - spaceCharsCount];
            ReadOnlySpan<char> reference = expression;
            for (int i = 0, j = 0; i < reference.Length; i++)
            {
                if (reference[i] == ' ') continue;
                expressionSpan[j++] = reference[i];
            }
            return BuildMathExpression(expressionSpan);
        }

        private static MathExpression BuildMathExpression(ReadOnlySpan<char> expressionSpan)
        {
            CheckParentheses(expressionSpan);
            (int operationsCount, int selfOperationsCount) = expressionSpan.CountOperationsAndSelfOperations();

            Span<Operation?> operations = stackalloc Operation?[operationsCount];
            Span<Number?> numbers = stackalloc Number?[operationsCount + 1];
            Span<SelfOperation?> selfOperations = stackalloc SelfOperation?[selfOperationsCount];

            var content = new Content(numbers, operations, selfOperations);
            
            if ((content.Numbers.Length + content.Operations.Length) % 2 == 0) throw new ArgumentException("The provided expression is not complete.");

            content.Build(expressionSpan);
            content.Process();
            return new MathExpression(content.Numbers[0]!.Value.Value, true);
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