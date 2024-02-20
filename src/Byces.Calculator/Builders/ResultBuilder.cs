using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Builders
{
    internal sealed class ResultBuilder
    {
        public ResultBuilder(CalculatorOptions options, 
            BuiltExpressions builtExpressions, CultureInfo? cultureInfo, 
            ConcurrentDictionary<int, Content> cachedExpressions)
        {
            _cachedExpressions = cachedExpressions;
            _options = options;
            _content = new Content();
            _contentBuilder = new ContentBuilder(_content, builtExpressions, cultureInfo);
            _expressionBuilder = new List<char>();
        }

        private readonly ConcurrentDictionary<int, Content> _cachedExpressions;

        private readonly CalculatorOptions _options;

        private readonly Content _content;

        private readonly ContentBuilder _contentBuilder;

        private readonly List<char> _expressionBuilder;

        public void Build(ReadOnlySpan<char> expressionSpan)
        {
            ReadOnlySpan<char> formattedExpressionSpan = GetFormattedExpression(expressionSpan);
            if (formattedExpressionSpan.IsEmpty) return;
            
            BuildContent(formattedExpressionSpan);
        }

        private ReadOnlySpan<char> GetFormattedExpression(ReadOnlySpan<char> expressionSpan)
        {
            if ((_options & CalculatorOptions.RemoveWhitespaceChecker) != 0) return expressionSpan;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(expressionSpan[i])) continue;
                _expressionBuilder.Add(expressionSpan[i]);
            }
            return CollectionsMarshal.AsSpan(_expressionBuilder);
        }

        private void BuildContent(ReadOnlySpan<char> expressionSpan)
        {
            if ((_options & CalculatorOptions.CacheExpressions) == 0)
            {
                _contentBuilder.Build(expressionSpan);
                _content.Process();
                return;
            }
            int hashCode = string.GetHashCode(expressionSpan, StringComparison.OrdinalIgnoreCase);
            if (_cachedExpressions.TryGetValue(hashCode, out Content? cachedContent))
            {
                _content.CopyValues(cachedContent);
                _content.Process();
                return;
            }
            _contentBuilder.Build(expressionSpan);
            cachedContent = new Content();
            _cachedExpressions.TryAdd(hashCode, cachedContent);
            
            if (_contentBuilder.InconstantResult)
            {
                cachedContent.CopyValues(_content);
                _content.Process();
                
                return;
            }
            _content.Process();
            cachedContent.CopyResult(_content);
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