using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Variables.Number
{
    internal sealed class Euler : VariableRepresentation
    {
        public override string StringRepresentation => "EULER";
        public override char CharRepresentation => 'E';

        public override Value GetValue() => Math.E;
    }
}