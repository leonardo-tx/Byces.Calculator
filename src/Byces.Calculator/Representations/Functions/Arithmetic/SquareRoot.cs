using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class SquareRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "SQRT";
        public override char CharRepresentation => '√';
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;

            if (number < 0) throw new ArithmeticExpressionException("Attempted to square root a negative number");
            return Math.Sqrt(number);
        }
    }
}