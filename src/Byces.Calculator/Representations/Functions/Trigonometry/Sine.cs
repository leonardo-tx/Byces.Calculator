using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class Sine : FunctionRepresentation
    {
        public override string StringRepresentation => "SIN";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double result = Math.Sin(variables[0].Double);
            return Math.Round(result, 15);
        }
    }
}