using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class SquareRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "SQRT";
        public override char CharRepresentation => '√';
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;
            return Math.Sqrt(number);
        }
    }
}