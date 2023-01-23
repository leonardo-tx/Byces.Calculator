using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
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
            throw new UnknownNumberExpressionException();
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