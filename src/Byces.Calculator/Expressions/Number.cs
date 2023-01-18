using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Number
    {
        internal Number(double value)
        {
            Value = value;
        }

        internal double Value { get; }

        internal static Number Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out Number number)) return number;
            throw new ArgumentException("The given expression has unknown numbers.");
        }

        internal static bool TryParse(ReadOnlySpan<char> span, out Number number)
        {
            if (double.TryParse(span, out double value) || SpecialNumberType.TryParse(span, out value))
            {
                number = new Number(value);
                return true;
            }
            number = default;
            return false;
        }
    }
}