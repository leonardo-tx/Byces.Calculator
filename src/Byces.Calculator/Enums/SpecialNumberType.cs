using Byces.Calculator.Enums.SpecialNumbers;
using System;
using System.Reflection;

namespace Byces.Calculator.Enums
{
    internal abstract class SpecialNumberType
    {
        public static readonly SpecialNumberType Pi = new PiType();
        public static readonly SpecialNumberType Euler = new EulerType();

        static SpecialNumberType()
        {
            Type type = typeof(SpecialNumberType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            SpecialNumberType[] specialNumbers = new SpecialNumberType[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                specialNumbers[i] = (SpecialNumberType)fields[i].GetValue(null)!;
            }
            _allSpecialNumbers = specialNumbers;
        }

        private static readonly ReadOnlyMemory<SpecialNumberType> _allSpecialNumbers;

        internal abstract string StringRepresentation { get; }

        internal abstract char CharRepresentation { get; }

        internal abstract double GetNumber();
        
        internal static double Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out double number)) return number;
            throw new ArgumentException("Could not parse the given number");
        }

        internal static double Parse(char character)
        {
            if (TryParse(character, out double number)) return number;
            throw new ArgumentException("Could not parse the given number");
        }

        internal static bool TryParse(ReadOnlySpan<char> span, out double number)
        {
            number = 0;
            ReadOnlySpan<char> validSourceSpan = GetValidSourceSpan(span, out bool isNegative);

            if (validSourceSpan.Length == 0) return false;
            if (validSourceSpan.Length == 1)
            {
                bool parseResult = TryParse(validSourceSpan[0], out number);
                if (isNegative) number *= -1;
                return parseResult;
            }
            ReadOnlySpan<SpecialNumberType> reference = _allSpecialNumbers.Span;

            for (int i = 0; i < reference.Length; i++)
            {
                if (!validSourceSpan.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                number = reference[i].GetNumber(); 
                
                if (isNegative) number *= -1;
                return true;
            }
            return false;
        }

        internal static bool TryParse(char character, out double number)
        {
            number = 0;
            if (character == '\0') return false;

            ReadOnlySpan<SpecialNumberType> reference = _allSpecialNumbers.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                number = reference[i].GetNumber(); return true;
            }
            return false;
        }

        private static ReadOnlySpan<char> GetValidSourceSpan(ReadOnlySpan<char> span, out bool isNegative)
        {
            isNegative = false;
            if (span.Length == 0) return span;

            switch (span[0])
            {
                case '+':
                    return span[1..];
                case '-':
                    isNegative = true;
                    return span[1..];
                default:
                    return span;
            };
        }
    }
}