using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class CubeRootItem : FunctionItem
    {
        public CubeRootItem(): base("CBRT")
        {
        }
        
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => Math.Cbrt(variables[0].Double);
    }
}