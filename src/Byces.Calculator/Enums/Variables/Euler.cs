using System;

namespace Byces.Calculator.Enums.Variables
{
    internal sealed class Euler : VariableRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "EULER";
        public override char CharRepresentation => 'E';

        internal override double GetNumber() => Math.E;
    }
}