using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums
{
    internal abstract class OperationType : ExpressionType<OperationType>
    {
        public static implicit operator int(OperationType operationType) => operationType.Value;
        public static explicit operator OperationType(int value) => GetItem(value);

        internal abstract OperationPriorityType Priority { get; }

        internal virtual Value Operate(Value firstValue, Value secondValue) => throw new NotSupportedException();
    }
}