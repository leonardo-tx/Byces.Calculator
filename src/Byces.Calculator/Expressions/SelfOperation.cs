using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions
{
    internal readonly struct SelfOperation
    {
        internal SelfOperation(Operation operation, int priority)
        {
            Operation = operation;
            Priority = priority;
        }

        internal Operation Operation { get; }

        internal int Priority { get; }
    }
}