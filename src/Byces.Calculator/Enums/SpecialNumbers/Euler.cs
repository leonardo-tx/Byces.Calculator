using System;

namespace Byces.Calculator.Enums.SpecialNumbers
{
    internal sealed class Euler : SpecialNumberType
    {
        protected override string StringRepresentation => "EULER";
        protected override char CharRepresentation => default;

        protected override double GetNumber() => Math.E;
    }
}