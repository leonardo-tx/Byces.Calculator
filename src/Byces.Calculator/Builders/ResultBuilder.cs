using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Byces.Calculator.Collections;
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
            _expressionBuilder = dependencies.HasWhitespaceCheck() ? new List<char>() : null!;
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
            if (!_dependencies.HasWhitespaceCheck()) return expressionSpan;
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
            if (_dependencies.CachedExpressions.TryGetValue(expressionSpan, out CachedContent? cachedContent))
            {
                cachedContent.CopyTo(_content);
                _content.Process();
                return;
            }
            _contentBuilder.Build(expressionSpan);
            cachedContent = new CachedContent();
            
            if (_contentBuilder.InconstantResult)
            {
                _content.CopyTo(cachedContent);
                _content.Process();
            }
            else
            {
                _content.Process();
                _content.CopyTo(cachedContent);
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