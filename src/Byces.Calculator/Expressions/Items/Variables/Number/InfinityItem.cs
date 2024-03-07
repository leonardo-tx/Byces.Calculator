namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class InfinityItem : NumberItem
    {
        public InfinityItem(): base("INFINITY", "∞")
        {
        }
        
        public override bool Pure => true;

        public override double GetValue() => double.PositiveInfinity;
    }
}