using Byces.Calculator.Enums.SpecialNumbers;
using System;
using System.Reflection;

namespace Byces.Calculator.Enums
{
    internal abstract class SpecialNumberType
    {
        public static readonly SpecialNumberType Pi = new Pi();
        public static readonly SpecialNumberType Euler = new Euler();

        static SpecialNumberType()
        {
            Type type = typeof(SpecialNumberType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            int maxStringSize = 0;
            SpecialNumberType[] specialNumbers = new SpecialNumberType[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var specialNumberType = (SpecialNumberType)fields[i].GetValue(null)!;
                if (specialNumberType.StringRepresentation.Length > maxStringSize) maxStringSize = specialNumberType.StringRepresentation.Length;

                specialNumbers[i] = specialNumberType;
            }
            _items = specialNumbers;
            MaxStringSize = maxStringSize;
        }

        private static readonly SpecialNumberType[] _items;

        protected abstract string StringRepresentation { get; }

        protected abstract char CharRepresentation { get; }

        protected abstract double GetNumber();

        internal static int MaxStringSize { get; }

        internal static bool TryParse(ReadOnlySpan<char> span, out double number)
        {
            ReadOnlySpan<char> validSourceSpan = GetValidSourceSpan(span, out bool isNegative);
            if (validSourceSpan.Length == 1)
            {
                bool parseResult = TryParse(validSourceSpan[0], out number);
                if (isNegative) number *= -1;
                return parseResult;
            }
            ReadOnlySpan<SpecialNumberType> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!validSourceSpan.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                number = reference[i].GetNumber(); 
                
                if (isNegative) number *= -1;
                return true;
            }
            number = double.NaN; return false;
        }

        internal static bool TryParse(char character, out double number)
        {
            if (character == '\0') { number = double.NaN; return false; }

            ReadOnlySpan<SpecialNumberType> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                number = reference[i].GetNumber(); return true;
            }
            number = double.NaN; return false;
        }

        private static ReadOnlySpan<char> GetValidSourceSpan(ReadOnlySpan<char> span, out bool isNegative)
        {
            switch (span[0])
            {
                case '+':
                    isNegative = false;
                    return span[1..];
                case '-':
                    isNegative = true;
                    return span[1..];
                default:
                    isNegative = false;
                    return span;
            };
        }
    }
}