using System;

namespace Byces.Calculator.Enums.SpecialNumbers
{
    internal sealed class PiType : SpecialNumberType
    {
        internal override string StringRepresentation => "PI";
        internal override char CharRepresentation => 'π';

        internal override double GetNumber() => Math.PI;
    }
}