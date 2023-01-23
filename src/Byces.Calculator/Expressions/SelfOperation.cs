using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions
{
    internal readonly struct SelfOperation
    {
        internal SelfOperation(int numberIndex, int selfOperation, int priority)
        {
            NumberIndex = numberIndex;
            Value = selfOperation;
            Priority = priority;
        }

        internal int NumberIndex { get; }

        internal int Value { get; }

        internal int Priority { get; }

        internal double Operate(double number) => SelfOperationType.GetSelfOperation(Value).Operate(number);
    }
}