using Byces.Calculator.Enums;
using System;
using System.Collections.Generic;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Content
    {
        internal Content(IList<Number> numbers, IList<Operation> operations, IList<int> priorities)
        {
            Numbers = numbers;
            Operations = operations;
            Priorities = priorities;
        }

        internal static readonly Content Empty = new Content(new List<Number>(1) { new Number(0) }, Array.Empty<Operation>(), Array.Empty<int>());

        internal IList<Number> Numbers { get; }

        internal IList<Operation> Operations { get; }

        internal IList<int> Priorities { get; }
    }
}