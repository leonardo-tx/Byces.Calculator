using System;

namespace Byces.Calculator.Expressions.Items.Functions.Arithmetic
{
    internal sealed class SquareRootItem : FunctionItem
    {
        public SquareRootItem(): base("SQRT", "√")
        {
        }
        
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;
            return Math.Sqrt(number);
        }
    }
}