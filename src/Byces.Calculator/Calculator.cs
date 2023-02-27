using Byces.Calculator.Expressions;
using Microsoft.Extensions.ObjectPool;
using System;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the math expression calculation functionality.
    /// </summary>
    public sealed class Calculator : ICalculator
    {
        internal Calculator(bool hasResultPool, bool hasWhiteSpaceRemover)
        {
            if (hasResultPool) _resultPool = ObjectPool.Create<StoredResult>();
            _contentPool = ObjectPool.Create<Content>();
            _hasWhiteSpaceRemover = hasWhiteSpaceRemover;
        }

        private readonly ObjectPool<Content> _contentPool;
        private readonly ObjectPool<StoredResult>? _resultPool;
        private readonly bool _hasWhiteSpaceRemover;

        /// <summary>
        /// Gets the <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string expression)
        {
            ReadOnlySpan<char> expressionSpan = expression;
            if (expressionSpan.IsEmpty || (_hasWhiteSpaceRemover && expressionSpan.IsWhiteSpace())) return new MathResult<double>(0, true);
            try
            {
                double result = GetResultFromExpression(expression, expressionSpan);
                return new MathResult<double>(result, true);
            }
            catch (Exception ex)
            {
                return new MathResult<double>(ex, double.NaN);
            }
        }

        private double GetResultFromExpression(string expression, ReadOnlySpan<char> expressionSpan)
        {
            if (_resultPool != null) { return GetResultFromExpressionWithResultPool(expression, expressionSpan); }
            var content = _contentPool.Get();
            try
            {
                content.Build(expressionSpan, _hasWhiteSpaceRemover);
                content.Process();

                return content.Numbers[0];
            }
            finally
            {
                content.Clear();
                _contentPool.Return(content);
            }
        }

        private double GetResultFromExpressionWithResultPool(string expression, ReadOnlySpan<char> expressionSpan)
        {
            var storedResult = _resultPool!.Get();
            if (expressionSpan.Equals(storedResult.Expression, StringComparison.Ordinal))
            {
                try { return storedResult.Result; }
                finally { _resultPool.Return(storedResult); }
            }

            var content = _contentPool.Get();
            try
            {
                content.Build(expressionSpan, _hasWhiteSpaceRemover);
                content.Process();

                storedResult.Expression = expression;
                storedResult.Result = content.Numbers[0];

                return content.Numbers[0];
            }
            finally
            {
                content.Clear();
                _contentPool.Return(content);
                _resultPool.Return(storedResult);
            }
        }
    }
}