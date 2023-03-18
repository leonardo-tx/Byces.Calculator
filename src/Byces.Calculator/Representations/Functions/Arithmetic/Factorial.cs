using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using MathNet.Numerics;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Factorial : FunctionRepresentation
    {
        public override string StringRepresentation => "FACT";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;
            if (number < 0) throw new ArithmeticExpressionException("Attempted to factorial a negative number.");

            double difference = number - Math.Floor(number);
            if (difference == 0) return SpecialFunctions.Factorial((int)number);

            return SpecialFunctions.Gamma(number + 1);
        }
    }
}