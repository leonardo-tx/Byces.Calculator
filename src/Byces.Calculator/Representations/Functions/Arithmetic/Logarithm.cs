using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Logarithm : FunctionRepresentation
    {
        public override string StringRepresentation => "LOG";
        public override int ParametersMax => 2;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return variables.Length == 1 
                ? Log10(variables[0].Double) 
                : LogCustom(variables[0].Double, variables[1].Double);
        }

        private static double Log10(double value)
        {
            return Math.Log10(value);
        }

        private static double LogCustom(double left, double right)
        {
            return Math.Log(left, right);
        }
    }
}