using Byces.Calculator.Enums;
using System;
using System.Collections.Generic;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Number
    {
        internal Number(double value, IList<Operation> operations)
        {
            Value = value;
            Operations = operations;
        }

        internal Number(double value)
        {
            Value = value;
            Operations = Array.Empty<Operation>();
        }

        internal double Value { get; }

        internal IList<Operation> Operations { get; }

        internal static Number Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out Number number)) return number;
            throw new ArgumentException("The given expression has unknown numbers.");
        }

        internal static Number Parse(ReadOnlySpan<char> span, IList<Operation> operations)
        {
            if (TryParse(span, operations, out Number number)) return number;
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

        internal static bool TryParse(ReadOnlySpan<char> span, IList<Operation> operations, out Number number)
        {
            if (double.TryParse(span, out double value) || SpecialNumberType.TryParse(span, out value))
            {
                number = new Number(value, operations);
                return true;
            }
            number = default;
            return false;
        }
    }
}