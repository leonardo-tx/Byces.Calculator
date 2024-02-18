using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Max : FunctionRepresentation
    {
        public override string StringRepresentation => "MAX";
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double max = variables[0].Double;
            for (int i = 1; i < variables.Length; i++)
            {
                if (variables[i].Double > max) max = variables[i].Double;
            }
            return max;
        }
    }
}