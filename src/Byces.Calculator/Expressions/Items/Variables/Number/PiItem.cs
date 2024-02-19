using System;

namespace Byces.Calculator.Expressions.Items.Variables.Number
{
    internal sealed class PiItem : VariableItem
    {
        public override string StringRepresentation => "PI";
        public override char CharRepresentation => 'π';
        
        public override bool Pure => true;

        public override Variable GetValue() => Math.PI;
    }
}