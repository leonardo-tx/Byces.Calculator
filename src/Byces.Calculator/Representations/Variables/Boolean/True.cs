using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Variables.Boolean
{
    internal sealed class True : VariableRepresentation
    {
        public override string StringRepresentation => "TRUE";

        public override Variable GetValue() => true;
    }
}