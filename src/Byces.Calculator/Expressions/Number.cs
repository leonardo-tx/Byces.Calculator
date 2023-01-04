using System;
using System.Collections.Generic;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Number
    {
        internal Number(double value, IList<SelfOperation> operations)
        {
            Value = value;
            Operations = operations;
        }

        internal Number(double value)
        {
            Value = value;
            Operations = Array.Empty<SelfOperation>();
        }

        internal double Value { get; }

        internal IList<SelfOperation> Operations { get; }
    }
}