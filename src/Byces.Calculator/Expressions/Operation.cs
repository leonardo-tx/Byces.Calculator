using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Operation
    {
        internal Operation(OperatorItem operation, int priority)
        {
            Value = operation;
            Priority = priority;
        }

        internal OperatorItem Value { get; }

        internal int Priority { get; }
    }
}