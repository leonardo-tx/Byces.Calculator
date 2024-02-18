using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Add : FunctionRepresentation
    {
        public override string StringRepresentation => "ADD";
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double result = 0;
            for (int i = 0; i < variables.Length; i++)
            {
                result += variables[i].Double;
            }
            return result;
        }
    }
}