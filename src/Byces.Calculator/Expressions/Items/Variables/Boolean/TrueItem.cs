namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class TrueItem : BooleanItem
    {
        public TrueItem(): base("TRUE")
        {
        }
        
        public override bool Pure => true;

        public override bool GetValue() => true;
    }
}