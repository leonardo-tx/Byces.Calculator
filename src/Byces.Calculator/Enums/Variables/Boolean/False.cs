using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Variables.Boolean
{
    internal sealed class False : VariableRepresentation
    {
        public override string StringRepresentation => "FALSE";

        public override Value GetValue() => false;
    }
}