using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Variables.Boolean
{
    internal sealed class True : VariableRepresentation
    {
        public override string StringRepresentation => "TRUE";

        public override Value GetValue() => true;
    }
}