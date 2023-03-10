using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Divide : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "DIV";
        public override char CharRepresentation => '/';
        internal override OperatorPriority Priority => OperatorPriority.Second;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (secondValue.Number == 0) throw new ArithmeticExpressionException("Attempted to divide by zero.");
            
            return firstValue.Number / secondValue.Number;
        }
    }
}