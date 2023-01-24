using System;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using Byces.Calculator.Extensions;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="MathResult"/> in a simplified way.
    /// </summary>
    public sealed class MathResultBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="MathResultBuilder"/> class.
        /// </summary>
        public MathResultBuilder()
        {
            Expression = string.Empty;
        }

        /// <summary>
        /// Gets or sets the expression of a <see cref="MathResultBuilder"/>.
        /// </summary>
        /// <returns>The stored expression of the builder, or <see cref="string.Empty"/> if none is set.</returns>
        public string Expression { get; set; }

        /// <summary>
        /// Sets the expression to be builded.
        /// </summary>
        public MathResultBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="MathResult"/> with the given information.
        /// </summary>
        /// <returns>The built result.</returns>
        public MathResult Build()
        {
            return GetMathResult(Expression);
        }

        /// <summary>
        /// Gets the <see cref="MathResult"/> without having to create a <see cref="MathResultBuilder"/> object.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public static MathResult GetMathResult(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return new MathResult(0, true);
            try
            {
                return FormatExpression(expression, 1024);
            }
            catch (Exception ex)
            {
                return new MathResult(ex);
            }
        }

        private static MathResult FormatExpression(string expression, int stackLimit)
        {
            int spaceCharsCount = expression.AsSpan().CountWhiteSpaces();
            if (spaceCharsCount == 0) return BuildMathExpression(expression, stackLimit);
            
            int size = expression.Length - spaceCharsCount;

            Span<char> expressionSpan = (size < stackLimit) ? stackalloc char[size] : new char[size];
            ReadOnlySpan<char> reference = expression;
            for (int i = 0, j = 0; i < reference.Length; i++)
            {
                if (char.IsWhiteSpace(reference[i])) continue;
                expressionSpan[j++] = reference[i];
            }
            return BuildMathExpression(expressionSpan, stackLimit);
        }

        private static MathResult BuildMathExpression(ReadOnlySpan<char> expressionSpan, int stackLimit)
        {
            CheckParentheses(expressionSpan);
            (int operationsCount, int selfOperationsCount) = expressionSpan.CountOperationsAndSelfOperations();

            Span<Operation?> operations = (operationsCount < stackLimit) ? stackalloc Operation?[operationsCount] : new Operation?[operationsCount];
            Span<Number?> numbers = (operationsCount + 1 < stackLimit) ? stackalloc Number?[operationsCount + 1] : new Number?[operationsCount + 1];
            Span<SelfOperation?> selfOperations = (selfOperationsCount < stackLimit) ? stackalloc SelfOperation?[selfOperationsCount] : new SelfOperation?[selfOperationsCount];

            var content = new Content(numbers, operations, selfOperations);

            content.Build(expressionSpan);
            content.Process();
            return new MathResult(content.Numbers[0]!.Value.Value, true);
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
            if (unclosedParentheses < 0) throw new MisplacedParenthesesExpressionException();
            if (unclosedParentheses > 0) throw new MissingParenthesesExpressionException();
        }
    }
}