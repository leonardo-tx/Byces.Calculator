namespace Byces.Calculator.Expressions.Items.Variables.Boolean
{
    internal sealed class FalseItem : BooleanItem
    {
        public FalseItem(): base("FALSE")
        {
        }
        
        public override bool Pure => true;

        public override bool GetValue() => false;
    }
}