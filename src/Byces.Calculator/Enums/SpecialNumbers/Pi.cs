using System;

namespace Byces.Calculator.Enums.SpecialNumbers
{
    internal sealed class Pi : SpecialNumberType
    {
        protected override string StringRepresentation => "PI";
        protected override char CharRepresentation => 'π';

        protected override double GetNumber() => Math.PI;
    }
}