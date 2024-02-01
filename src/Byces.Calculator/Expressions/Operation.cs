using Byces.Calculator.Representations;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Operation
    {
        internal Operation(OperatorRepresentation operation, int priority)
        {
            Value = operation;
            Priority = priority;
        }

        internal OperatorRepresentation Value { get; }

        internal int Priority { get; }
    }
}