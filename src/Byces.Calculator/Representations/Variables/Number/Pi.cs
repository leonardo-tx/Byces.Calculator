using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Variables.Number
{
    internal sealed class Pi : VariableRepresentation
    {
        public override string StringRepresentation => "PI";
        public override char CharRepresentation => 'π';

        public override Variable GetValue() => Math.PI;
    }
}