namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal class NotANumberItem : NumberItem
    {
        public NotANumberItem(): base("NAN")
        {
        }
        
        public override bool Pure => true;

        public override double GetValue() => double.NaN;
    }
}