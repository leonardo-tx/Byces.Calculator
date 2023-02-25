using System;
using Byces.Calculator.Expressions;
using Byces.Calculator.Extensions;
using Microsoft.Extensions.ObjectPool;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="MathResult"/> in a simplified way.
    /// </summary>
    public sealed class MathResultBuilder
    {
        private readonly static ObjectPool<Content> contentPool = ObjectPool.Create<Content>();
        private readonly static ObjectPool<StoredResult> resultPool = ObjectPool.Create<StoredResult>();

        private const int StackLimit = 1024;

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
            try
            {
                return FormatExpression(expression);
            }
            catch (Exception ex)
            {
                return new MathResult(ex);
            }
        }

        private static MathResult FormatExpression(string expression)
        {
            ReadOnlySpan<char> rawExpressionSpan = expression;

            int spaceCharsCount = rawExpressionSpan.CountWhiteSpaces();
            if (spaceCharsCount == 0) return BuildMathExpression(rawExpressionSpan, expression);
            
            int size = expression.Length - spaceCharsCount;

            Span<char> expressionSpan = (size < StackLimit) ? stackalloc char[size] : new char[size];
            for (int i = 0, j = 0; i < rawExpressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(rawExpressionSpan[i])) continue;
                expressionSpan[j++] = rawExpressionSpan[i];
            }
            return BuildMathExpression(expressionSpan, expression);
        }

        private static MathResult BuildMathExpression(ReadOnlySpan<char> expressionSpan, string expression)
        {
            if (expressionSpan.IsEmpty) return new MathResult(0, true);
            
            var lastResult = resultPool.Get();
            if (lastResult.Expression == expression) { try { return new MathResult(lastResult.Result, true); } finally { resultPool.Return(lastResult); } }

            var content = contentPool.Get();
            try
            {
                content.Build(expressionSpan);
                content.Process();
                
                lastResult.Expression = expression;
                lastResult.Result = content.Numbers[0];

                return new MathResult(lastResult.Result, true);
            }
            finally
            {
                content.Clear();
                contentPool.Return(content);
                resultPool.Return(lastResult);
            }
        }
    }
}