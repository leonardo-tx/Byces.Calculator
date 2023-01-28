namespace Byces.Calculator.Expressions
{
    internal readonly struct Operation
    {
        internal Operation(int operation, int priority)
        {
            Value = operation;
            Priority = priority;
        }

        internal int Value { get; }

        internal int Priority { get; }
    }
}