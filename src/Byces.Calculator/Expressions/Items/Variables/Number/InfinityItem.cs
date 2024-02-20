namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class InfinityItem : NumberItem
    {
        public override char CharRepresentation => '∞';
        public override string StringRepresentation => "INFINITY";
        
        public override bool Pure => true;

        public override double GetValue() => double.PositiveInfinity;
    }
}