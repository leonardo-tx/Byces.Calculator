using System;
using System.Reflection;
using Byces.Calculator.Enums.SelfOperations;

namespace Byces.Calculator.Enums
{
    internal abstract class FunctionType
    {
        public static implicit operator int(FunctionType functionType) => functionType.Value;

        public static readonly FunctionType Factorial = new Factorial();
        public static readonly FunctionType SquareRoot = new SquareRoot();
        public static readonly FunctionType CubeRoot = new CubeRoot();
        public static readonly FunctionType Cosine = new Cosine();
        public static readonly FunctionType Sine = new Sine();
        public static readonly FunctionType Tangent = new Tangent();
        public static readonly FunctionType CosineHyperbolic = new CosineHyperbolic();
        public static readonly FunctionType SineHyperbolic = new SineHyperbolic();
        public static readonly FunctionType TangentHyperbolic = new TangentHyperbolic();
        public static readonly FunctionType Radian = new Radian();
        public static readonly FunctionType Logarithm = new Logarithm();

        static FunctionType()
        {
            Type type = typeof(FunctionType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            
            FunctionType[] allFunctions = new FunctionType[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var functionType = (FunctionType)fields[i].GetValue(null)!;
                if (allFunctions[functionType.Value] != null) throw new Exception("There are fields with duplicate int values");

                allFunctions[functionType.Value] = functionType;
            }
            _allFunctions = allFunctions;
        }

        private static readonly ReadOnlyMemory<FunctionType> _allFunctions;

        protected abstract int Value { get; }

        internal virtual int AdditionalCheck => 0;

        internal abstract string StringRepresentation { get; }

        internal abstract char CharRepresentation { get; }

        internal abstract double Operate(double number);

        internal static bool TryParse(ReadOnlySpan<char> span, out FunctionType functionType)
        {
            if (span.Length == 1) return TryParse(span[0], out functionType);

            ReadOnlySpan<FunctionType> reference = _allFunctions.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                functionType = reference[i]; return true;
            }
            functionType = Factorial; return false;
        }

        internal static bool TryParse(char character, out FunctionType operationType)
        {
            if (character == '\0') { operationType = Factorial; return false; }

            ReadOnlySpan<FunctionType> reference = _allFunctions.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                operationType = reference[i]; return true;
            }
            operationType = Factorial; return false;
        }

        internal static FunctionType GetSelfOperation(int value) => _allFunctions.Span[value];
    }
}