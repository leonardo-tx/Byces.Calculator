using Byces.Calculator.Expressions;
using Byces.Calculator.Interfaces;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Byces.Calculator.Builders;
using Byces.Calculator.Enums;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the math expression calculation functionality.
    /// </summary>
    public sealed class Calculator : ICalculator
    {
        internal Calculator(CultureInfo? cultureInfo, CalculatorOptions options)
        {
            _cultureInfo = cultureInfo;
            _options = options;
            _resultBuilderPool = ObjectPool.Create<ResultBuilder>();
        }

        private readonly CultureInfo? _cultureInfo;

        private readonly CalculatorOptions _options;

        private readonly ObjectPool<ResultBuilder> _resultBuilderPool;

        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
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
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
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
                resultBuilder.Build(rawExpressionSpan, _cultureInfo ?? Thread.CurrentThread.CurrentCulture, _options);
                return resultBuilder.GetResult();
            }
            finally
            {
                resultBuilder.Clear();
                _resultBuilderPool.Return(resultBuilder);
            }
        }
    }
}