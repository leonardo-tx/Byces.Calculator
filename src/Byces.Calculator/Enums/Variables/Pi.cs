using System;

namespace Byces.Calculator.Enums.Variables
{
    internal sealed class Pi : VariableRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "PI";
        public override char CharRepresentation => 'π';

        internal override double GetNumber() => Math.PI;
    }
}