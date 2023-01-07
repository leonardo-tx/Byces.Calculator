using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Operation
    {
        internal Operation(OperationType operation, int priority)
        {
            Value = operation;
            Priority = priority;
        }

        internal OperationType Value { get; }

        internal int Priority { get; }
    }
}