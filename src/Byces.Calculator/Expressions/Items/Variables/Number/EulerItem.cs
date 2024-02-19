using System;

namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class EulerItem : VariableItem
    {
        public override string StringRepresentation => "EULER";
        public override char CharRepresentation => 'E';
        
        public override bool Pure => true;

        public override Variable GetValue() => Math.E;
    }
}