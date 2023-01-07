using System;
using System.Collections.Generic;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Number
    {
        internal Number(double value, IList<Operation> operations)
        {
            Value = value;
            Operations = operations;
        }

        internal Number(double value)
        {
            Value = value;
            Operations = Array.Empty<Operation>();
        }

        internal double Value { get; }

        internal IList<Operation> Operations { get; }
    }
}