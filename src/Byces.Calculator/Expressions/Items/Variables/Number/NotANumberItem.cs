namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal class NotANumberItem : VariableItem
    {
        public override string StringRepresentation => "NAN";
        
        public override bool Pure => true;

        public override Variable GetValue() => double.NaN;
    }
}