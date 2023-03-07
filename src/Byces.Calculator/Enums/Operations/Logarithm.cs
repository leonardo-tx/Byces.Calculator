using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Logarithm : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "LOG";
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (firstValue.Number <= 1) throw new ArithmeticExpressionException($"Attempted to base {firstValue.Number} on a logarithm");
            if (secondValue.Number <= 0) throw new ArithmeticExpressionException($"Attempted to log (base {firstValue.Number}) of {secondValue.Number}");

            return Math.Log(secondValue.Number, firstValue.Number);
        }
    }
}