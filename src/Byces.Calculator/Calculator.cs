using Byces.Calculator.Expressions;
using Byces.Calculator.Interfaces;
using Microsoft.Extensions.ObjectPool;
using System;
using Byces.Calculator.Builders;
using Byces.Calculator.Factories;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the math expression calculation functionality.
    /// </summary>
    public sealed class Calculator : ICalculator
    {
        internal Calculator(CalculatorDependencies dependencies)
        {
            _dependencies = dependencies;
            _resultBuilderPool = new DefaultObjectPool<ResultBuilder>(new ResultBuilderFactory(dependencies));
        }

        private readonly CalculatorDependencies _dependencies;

        private readonly ObjectPool<ResultBuilder> _resultBuilderPool;

        /// <summary>
        /// Gets the number of expressions stored in the current calculator.
        /// </summary>
        public int CachedCount => _dependencies.CachedExpressions?.Count ?? 0;

        /// <summary>
        /// Releases all cache used to store expressions, if the option is enabled.
        /// </summary>
        public void FreeExpressionsCache()
        {
            if (!_dependencies.HasCachedExpressions()) return;
            _dependencies.CachedExpressions.Free();
        }

        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string? expression)
        {
            if (expression == null) return new MathResult<double>(0, true);
            return GetDoubleResult(expression.AsSpan());
        }
        
        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(ReadOnlySpan<char> expression)
        {
            ResultBuilder resultBuilder = _resultBuilderPool.Get();
            try
            {
                resultBuilder.Build(expression);
                return new MathResult<double>(resultBuilder.GetResult().Number, true);
            }
            catch (Exception ex)
            {
                return new MathResult<double>(ex, double.NaN);
            }
            finally
            {
                _resultBuilderPool.Return(resultBuilder);
            }
        }

        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(string? expression)
        {
            if (expression == null) return new MathResult<bool>(false, true);
            return GetBooleanResult(expression.AsSpan());
        }
        
        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(ReadOnlySpan<char> expression)
        {
            ResultBuilder resultBuilder = _resultBuilderPool.Get();
            try
            {
                resultBuilder.Build(expression);
                return new MathResult<bool>(resultBuilder.GetResult().Boolean, true);
            }
            catch (Exception ex)
            {
                return new MathResult<bool>(ex, false);
            }
            finally
            {
                _resultBuilderPool.Return(resultBuilder);
            }
        }
    }
}