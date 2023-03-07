using System;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Results;
using Microsoft.Extensions.ObjectPool;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="MathResult{T}"/> in a simplified way.
    /// </summary>
    [Obsolete("This class is deprecated and will be removed in some future release. Use this class instead: CalculatorBuilder.")]
    public sealed class MathResultBuilder
    {
        private readonly static ObjectPool<Content> _contentPool = ObjectPool.Create<Content>();
        private readonly static ObjectPool<ContentResult> _resultPool = ObjectPool.Create<ContentResult>();

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
        /// Builds the <see cref="MathResult{T}"/> with the given information.
        /// </summary>
        /// <returns>The built result.</returns>
        public MathResult<double> Build()
        {
            return GetMathResult(Expression);
        }

        /// <summary>
        /// Gets the <see cref="MathResult{T}"/> without having to create a <see cref="MathResultBuilder"/> object.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public static MathResult<double> GetMathResult(string expression)
        {
            try
            {
                return BuildMathResult(expression);
            }
            catch (Exception ex)
            {
                return new MathResult<double>(ex, double.NaN);
            }
        }

        private static MathResult<double> BuildMathResult(string expression)
        {
            ReadOnlySpan<char> expressionSpan = expression;
            if (expressionSpan.IsEmpty || expressionSpan.IsWhiteSpace()) return new MathResult<double>(0, true);

            Content content = _contentPool.Get();
            try
            {
                content.Build(expressionSpan, ResultType.Number);
                content.Process();

                if (content.Values.Count > 1 || content.Values[0].ResultType != ResultType.Number) return new MathResult<double>(0, true);
                return new MathResult<double>(content.Values[0].Number, true);
            }
            catch (Exception ex)
            {
                return new MathResult<double>(ex, double.NaN);
            }
            finally
            {
                content.Clear();
                _contentPool.Return(content);
            }
        }
    }
}