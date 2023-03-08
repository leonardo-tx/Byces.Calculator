using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Interfaces;
using Microsoft.Extensions.ObjectPool;
using System;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the math expression calculation functionality.
    /// </summary>
    public sealed class Calculator : ICalculator
    {
        internal Calculator()
        {
            _contentPool = ObjectPool.Create<Content>();
        }

        private readonly ObjectPool<Content> _contentPool;

        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string expression)
        {
            ReadOnlySpan<char> expressionSpan = expression;
            if (expressionSpan.IsEmpty || expressionSpan.IsWhiteSpace()) return new MathResult<double>(0, true);

            Content content = _contentPool.Get();
            try
            {
                content.Build(expressionSpan);
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

        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(string expression)
        {
            ReadOnlySpan<char> expressionSpan = expression;
            if (expressionSpan.IsEmpty || expressionSpan.IsWhiteSpace()) return new MathResult<bool>(false, true);

            Content content = _contentPool.Get();
            try
            {
                content.Build(expressionSpan);
                content.Process();
                
                if (content.Values.Count > 1 || content.Values[0].ResultType != ResultType.Boolean) return new MathResult<bool>(false, true);
                return new MathResult<bool>(content.Values[0].Boolean, true);
            }
            catch (Exception ex)
            {
                return new MathResult<bool>(ex, false);
            }
            finally
            {
                content.Clear();
                _contentPool.Return(content);
            }
        }
    }
}