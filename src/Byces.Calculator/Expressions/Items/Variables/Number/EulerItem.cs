using System;

namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class EulerItem : NumberItem
    {
        public override string StringRepresentation => "EULER";
        public override char CharRepresentation => 'E';
        
        public override bool Pure => true;

        public override double GetValue() => Math.E;
    }
}