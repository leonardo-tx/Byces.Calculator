using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Divide : OperationType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "DIV";
        protected override char CharRepresentation => '/';
        internal override OperationPriorityType Priority => OperationPriorityType.Second;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (secondValue.Number == 0) throw new ArithmeticExpressionException("Attempted to divide by zero.");
            
            return firstValue.Number / secondValue.Number;
        }
    }
}