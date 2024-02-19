using System;

namespace Byces.Calculator.Expressions.Items.Functions.Trigonometry
{
    internal sealed class TangentItem : FunctionItem
    {
        public override string StringRepresentation => "TAN";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;
            double degree = number * 180 / Math.PI;
            if (degree % 360.00 == 90.00) return double.NaN;

            double result = Math.Tan(number);
            return Math.Round(result, 15);
        }
    }
}