using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Variables.Number
{
    internal sealed class Infinity : VariableRepresentation
    {
        public override char CharRepresentation => '∞';
        public override string StringRepresentation => "INFINITY";

        public override Variable GetValue() => double.PositiveInfinity;
    }
}