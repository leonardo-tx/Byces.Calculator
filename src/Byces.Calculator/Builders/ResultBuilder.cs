using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Byces.Calculator.Representations;

namespace Byces.Calculator.Builders
{
    internal sealed class ResultBuilder
    {
        public ResultBuilder()
        {
            _content = new Content();
            _contentBuilder = new ContentBuilder(_content);
            _expressionBuilder = new List<char>();
            _cachedExpressions = new Dictionary<int, Content>();
        }

        private readonly Content _content;

        private readonly ContentBuilder _contentBuilder;

        private readonly List<char> _expressionBuilder;

        private readonly Dictionary<int, Content> _cachedExpressions;

        public void Build(ReadOnlySpan<char> expressionSpan, RepresentationsCollection representationsCollection, CultureInfo cultureInfo, CalculatorOptions options)
        {
            ReadOnlySpan<char> formattedExpressionSpan = GetFormattedExpression(expressionSpan, options);
            if (formattedExpressionSpan.IsEmpty) return;
            
            BuildContent(formattedExpressionSpan, representationsCollection, cultureInfo, options);
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

        private void BuildContent(ReadOnlySpan<char> expressionSpan, RepresentationsCollection representationsCollection, CultureInfo cultureInfo, CalculatorOptions options)
        {
            if ((options & CalculatorOptions.CacheExpressions) == 0)
            {
                _contentBuilder.Build(expressionSpan, representationsCollection, cultureInfo);
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
            _contentBuilder.Build(expressionSpan, representationsCollection, cultureInfo);
            cachedContent = new Content();
            _cachedExpressions.Add(hashCode, cachedContent);
            
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