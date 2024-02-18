using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Variables.Boolean
{
    internal sealed class False : VariableRepresentation
    {
        public override string StringRepresentation => "FALSE";
        
        public override bool Pure => true;

        public override Variable GetValue() => false;
    }
}