using System.Collections.Generic;
using Byces.Calculator.Cache.Variables;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Cache
{
    internal sealed class CachedContent
    {
        internal List<CachedVariable> Variables { get; } = new();

        internal List<Operation> Operations { get; } = new();

        internal List<Function> Functions { get; } = new();

        internal void CopyTo(Content content)
        {
            content.Operations.AddRange(Operations);
            content.Functions.AddRange(Functions);
            for (int i = 0; i < Variables.Count; i++)
            {
                content.Variables.Add(Variables[i].GetVariable());
            }
        }
    }
}