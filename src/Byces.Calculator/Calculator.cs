using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Interfaces;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            _builderPool = ObjectPool.Create<ContentBuilder>();
            _expressionPool = ObjectPool.Create<List<char>>();
        }

        private readonly ObjectPool<Content> _contentPool;
        private readonly ObjectPool<ContentBuilder> _builderPool;
        private readonly ObjectPool<List<char>> _expressionPool;

        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string expression)
        {
            try
            {
                Variable result = GetResult(expression, VariableType.Number);
                return new MathResult<double>(result._number, true);
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
                Variable result = GetResult(expression, VariableType.Boolean);
                return new MathResult<bool>(result._boolean, true);
            }
            catch (Exception ex)
            {
                return new MathResult<bool>(ex, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Variable GetResult(ReadOnlySpan<char> rawExpressionSpan, VariableType expectedType)
        {
            if (rawExpressionSpan.IsEmpty || rawExpressionSpan.IsWhiteSpace()) return default;
            
            List<char> expression = _expressionPool.Get();
            for (int i = 0; i < rawExpressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(rawExpressionSpan[i])) continue;
                expression.Add(rawExpressionSpan[i]);
            }
            Content content = _contentPool.Get();
            ContentBuilder builder = _builderPool.Get();
            try
            {
                builder.Build(content, CollectionsMarshal.AsSpan(expression));
                content.Process();

                if (content.Variables.Count != 1 || content.Variables[0].Type != expectedType) return default;
                return content.Variables[0];
            }
            finally
            {
                content.Clear();
                builder.Clear();
                expression.Clear();
                
                _contentPool.Return(content);
                _builderPool.Return(builder);
                _expressionPool.Return(expression);
            }
        }
    }
}