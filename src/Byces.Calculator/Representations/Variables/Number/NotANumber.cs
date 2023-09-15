using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Variables.Number
{
    internal class NotANumber : VariableRepresentation
    {
        public override string StringRepresentation => "NAN";

        public override Variable GetValue() => double.NaN;
    }
}