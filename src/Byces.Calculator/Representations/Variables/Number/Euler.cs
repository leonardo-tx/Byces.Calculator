using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Variables.Number
{
    internal sealed class Euler : VariableRepresentation
    {
        public override string StringRepresentation => "EULER";
        public override char CharRepresentation => 'E';
        
        public override bool Pure => true;

        public override Variable GetValue() => Math.E;
    }
}