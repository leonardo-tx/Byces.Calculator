using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Builders
{
    internal sealed class ResultBuilder
    {
        public ResultBuilder(CalculatorDependencies dependencies)
        {
            _dependencies = dependencies;
            _content = new Content();
            _contentBuilder = new ContentBuilder(_content, dependencies);
            _expressionBuilder = new List<char>();
        }

        private readonly CalculatorDependencies _dependencies;

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
            if ((_dependencies.Options & CalculatorOptions.RemoveWhitespaceChecker) != 0) return expressionSpan;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(expressionSpan[i])) continue;
                _expressionBuilder.Add(expressionSpan[i]);
            }
            return CollectionsMarshal.AsSpan(_expressionBuilder);
        }

        private void BuildContent(ReadOnlySpan<char> expressionSpan)
        {
            if (!_dependencies.HasCachedExpressions())
            {
                _contentBuilder.Build(expressionSpan);
                _content.Process();
                return;
            }
            if (_dependencies.CachedExpressions.TryGetContent(expressionSpan, out Content? cachedContent))
            {
                _content.CopyValues(cachedContent);
                _content.Process();
                return;
            }
            _contentBuilder.Build(expressionSpan);
            cachedContent = new Content();
            
            if (_contentBuilder.InconstantResult)
            {
                cachedContent.CopyValues(_content);
                _content.Process();
            }
            else
            {
                _content.Process();
                cachedContent.CopyResult(_content);
            }
            _dependencies.CachedExpressions.Add(expressionSpan, cachedContent);
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