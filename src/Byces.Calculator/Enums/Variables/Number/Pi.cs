using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Variables.Number
{
    internal sealed class Pi : VariableRepresentation
    {
        public override string StringRepresentation => "PI";
        public override char CharRepresentation => 'π';

        public override Value GetValue() => Math.PI;
    }
}