using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Function
    {
        internal Function(int numberIndex, int function, int priority)
        {
            NumberIndex = numberIndex;
            Value = function;
            Priority = priority;
        }

        internal int NumberIndex { get; }

        internal int Value { get; }

        internal int Priority { get; }

        internal Value Operate(Value value) => ((FunctionType)Value).Operate(value);

        internal Value Operate(ReadOnlySpan<Value> values) => ((FunctionType)Value).Operate(values);
    }
}