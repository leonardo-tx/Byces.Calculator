using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class CubeRoot : FunctionRepresentation
    {
        public override string StringRepresentation => "CBRT";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Cbrt(variables[0].Double);
    }
}