using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions
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

        public void Build(ReadOnlySpan<char> rawExpressionSpan, CultureInfo cultureInfo)
        {
            for (int i = 0; i < rawExpressionSpan.Length; i++)
            {
                if (char.IsWhiteSpace(rawExpressionSpan[i])) continue;
                _expressionBuilder.Add(rawExpressionSpan[i]);
            }
            ReadOnlySpan<char> formattedExpressionSpan = CollectionsMarshal.AsSpan(_expressionBuilder);
            if (formattedExpressionSpan.IsEmpty) return;
            
            _contentBuilder.Build(formattedExpressionSpan, cultureInfo);
            _content.Process();
        }

        public Variable GetResult(VariableType expectedType)
        {
            if (_content.Variables.Count != 1 || _content.Variables[0].Type != expectedType) return default;
            return _content.Variables[0];
        }
        
        public void Clear()
        {
            _content.Clear();
            _contentBuilder.Clear();
            _expressionBuilder.Clear();
        }
    }
}