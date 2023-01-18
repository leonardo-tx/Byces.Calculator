namespace Byces.Calculator.Expressions
{
    internal readonly struct SelfOperation
    {
        internal SelfOperation(Operation operation, int numberIndex)
        {
            NumberIndex = numberIndex;
            Operation = operation;
        }

        internal int NumberIndex { get; }

        internal Operation Operation { get; }
    }
}