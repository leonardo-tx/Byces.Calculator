using System;

namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class EulerItem : NumberItem
    {
        public EulerItem(): base("EULER", "E")
        {
        }
        
        public override bool Pure => true;

        public override double GetValue() => Math.E;
    }
}