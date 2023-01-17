using Byces.Calculator.Enums;

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

        internal double Operate(double firstNumber, double secondNumber) => OperationType.FromValue(Value).Operate(firstNumber, secondNumber);
        internal double Operate(double number) => OperationType.FromValue(Value).Operate(number);
    }
}