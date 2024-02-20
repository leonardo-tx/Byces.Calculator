namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class FalseItem : BooleanItem
    {
        public override string StringRepresentation => "FALSE";
        
        public override bool Pure => true;

        public override bool GetValue() => false;
    }
}