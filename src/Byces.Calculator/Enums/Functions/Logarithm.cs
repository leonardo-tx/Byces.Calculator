using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Logarithm : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "LOG";

        public override Value Operate(Value value)
        {
            if (value.Number <= 0) throw new ArithmeticExpressionException($"Attempted to log (base 10) of {value.Number}");
            return Math.Log10(value.Number);
        }

        public override Value Operate(ReadOnlySpan<Value> values)
        {
            if (values.Length > 2) throw new InvalidArgumentExpressionException();
            double firstNumber = values[0].Number;
            double secondNumber = values[1].Number;
            
            if (secondNumber <= 1) throw new ArithmeticExpressionException($"Attempted to base {secondNumber} on a logarithm");
            if (firstNumber <= 0) throw new ArithmeticExpressionException($"Attempted to log (base {secondNumber}) of {firstNumber}");
            
            return Math.Log(firstNumber, secondNumber);
        }
    }
}