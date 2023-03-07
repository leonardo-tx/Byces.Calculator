using System;

namespace Byces.Calculator.Enums.Variables
{
    internal sealed class Pi : VariableType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "PI";
        protected override char CharRepresentation => 'π';

        internal override double GetNumber() => Math.PI;
    }
}