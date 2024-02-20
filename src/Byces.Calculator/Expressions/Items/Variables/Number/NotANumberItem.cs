namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal class NotANumberItem : NumberItem
    {
        public override string StringRepresentation => "NAN";
        
        public override bool Pure => true;

        public override double GetValue() => double.NaN;
    }
}