namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class TrueItem : VariableItem
    {
        public override string StringRepresentation => "TRUE";
        
        public override bool Pure => true;

        public override Variable GetValue() => true;
    }
}