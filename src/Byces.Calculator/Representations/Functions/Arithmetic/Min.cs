using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Min : FunctionRepresentation
    {
        public override string StringRepresentation => "MIN";

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double min = variables[0].Double;
            for (int i = 1; i < variables.Length; i++)
            {
                if (variables[i].Double < min) min = variables[i].Double;
            }
            return min;
        }
    }
}