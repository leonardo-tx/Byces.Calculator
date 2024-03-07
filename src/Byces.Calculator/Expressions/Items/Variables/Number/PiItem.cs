using System;

namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class PiItem : NumberItem
    {
        public PiItem(): base("PI", "π")
        {
        }
        
        public override bool Pure => true;

        public override double GetValue() => Math.PI;
    }
}