using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class Cosine : FunctionRepresentation
    {
        public override string StringRepresentation => "COS";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double result = Math.Cos(variables[0].Double);
            return Math.Round(result, 15);
        }
    }
}