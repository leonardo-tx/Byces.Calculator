using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Logarithm : FunctionType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "LOG";

        public override Value Operate(Value value)
        {
            if (value.Number <= 0) throw new ArithmeticExpressionException($"Attempted to log (base 10) of {value.Number}");
            return Math.Log10(value.Number);
        }
    }
}