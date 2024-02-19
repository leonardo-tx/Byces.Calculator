using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Builders
{
    internal sealed class ResultBuilder
    {
        public ResultBuilder()
        {
            _content = new Content();
            _contentBuilder = new ContentBuilder(_content);
            _expressionBuilder = new List<char>();
        }

        private readonly Content _content;

        private readonly ContentBuilder _contentBuilder;

        private readonly List<char> _expressionBuilder;

        public void Build(ReadOnlySpan<char> expressionSpan, Calculator calculator)
        {
            ReadOnlySpan<char> formattedExpressionSpan = GetFormattedExpression(expressionSpan, calculator.Options);
            if (formattedExpressionSpan.IsEmpty) return;
            
            BuildContent(formattedExpressionSpan, calculator);
        }

        private ReadOnlySpan<char> GetFormattedExpression(ReadOnlySpan<char> expressionSpan, CalculatorOptions options)
        {
            if ((options & CalculatorOptions.RemoveWhitespaceChecker) != 0) return expressionSpan;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(expressionSpan[i])) continue;
                _expressionBuilder.Add(expressionSpan[i]);
            }
            return CollectionsMarshal.AsSpan(_expressionBuilder);
        }

        private void BuildContent(ReadOnlySpan<char> expressionSpan, Calculator calculator)
        {
            if ((calculator.Options & CalculatorOptions.CacheExpressions) == 0)
            {
                _contentBuilder.Build(expressionSpan, calculator.BuiltExpressions, calculator.CultureInfo);
                _content.Process();
                return;
            }
            int hashCode = string.GetHashCode(expressionSpan, StringComparison.OrdinalIgnoreCase);
            if (calculator.CachedExpressions.TryGetValue(hashCode, out Content? cachedContent))
            {
                _content.CopyValues(cachedContent);
                _content.Process();
                return;
            }
            _contentBuilder.Build(expressionSpan, calculator.BuiltExpressions, calculator.CultureInfo);
            cachedContent = new Content();
            calculator.CachedExpressions.TryAdd(hashCode, cachedContent);
            
            if (_contentBuilder.InconstantResult)
            {
                cachedContent.CopyValues(_content);
                _content.Process();
                
                return;
            }
            _content.Process();
            cachedContent.CopyValues(_content);
        }

        public Variable GetResult()
        {
            return _content.Variables.Count != 1 ? default : _content.Variables[0];
        }
        
        public void Clear()
        {
            _content.Clear();
            _contentBuilder.Clear();
            _expressionBuilder.Clear();
        }
    }
}