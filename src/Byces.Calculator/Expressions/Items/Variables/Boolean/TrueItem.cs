namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class TrueItem : BooleanItem
    {
        public override string StringRepresentation => "TRUE";
        
        public override bool Pure => true;

        public override bool GetValue() => true;
    }
}