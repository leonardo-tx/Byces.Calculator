using System;

namespace Byces.Calculator.Enums.SpecialNumbers
{
    internal sealed class EulerType : SpecialNumberType
    {
        internal override string StringRepresentation => "EULER";
        internal override char CharRepresentation => default;

        internal override double GetNumber() => Math.E;
    }
}