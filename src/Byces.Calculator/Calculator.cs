using Byces.Calculator.Expressions;
using Byces.Calculator.Interfaces;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Byces.Calculator.Builders;
using Byces.Calculator.Enums;
using Byces.Calculator.Factories;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the math expression calculation functionality.
    /// </summary>
    public sealed class Calculator : ICalculator
    {
        internal Calculator(BuiltExpressions builtExpressions, CultureInfo? cultureInfo, CalculatorOptions options)
        {
            _resultBuilderPool = new DefaultObjectPool<ResultBuilder>
            (
                new ResultBuilderFactory(options, builtExpressions, cultureInfo)
            );
        }

        private readonly ObjectPool<ResultBuilder> _resultBuilderPool;

        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string expression)
        {
            try
            {
                Variable result = GetResult(expression);
                return new MathResult<double>(result.Number, true);
            }
            catch (Exception ex)
            {
                return new MathResult<double>(ex, double.NaN);
            }
        }

        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(string expression)
        {
            try
            {
                Variable result = GetResult(expression);
                return new MathResult<bool>(result.Boolean, true);
            }
            catch (Exception ex)
            {
                return new MathResult<bool>(ex, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Variable GetResult(ReadOnlySpan<char> rawExpressionSpan)
        {
            ResultBuilder resultBuilder = _resultBuilderPool.Get();
            try
            {
                resultBuilder.Build(rawExpressionSpan);
                return resultBuilder.GetResult();
            }
            finally
            {
                _resultBuilderPool.Return(resultBuilder);
            }
        }
    }
}