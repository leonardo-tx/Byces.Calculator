namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class FalseItem : VariableItem
    {
        public override string StringRepresentation => "FALSE";
        
        public override bool Pure => true;

        public override Variable GetValue() => false;
    }
}