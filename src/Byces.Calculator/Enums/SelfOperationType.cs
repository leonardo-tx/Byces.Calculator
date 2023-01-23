using System;
using System.Reflection;
using Byces.Calculator.Enums.SelfOperations;

namespace Byces.Calculator.Enums
{
    internal abstract class SelfOperationType
    {
        public static implicit operator int(SelfOperationType selfOperationType) => selfOperationType.Value;

        public static readonly SelfOperationType Factorial = new FactorialType();
        public static readonly SelfOperationType SquareRoot = new SquareRootType();
        public static readonly SelfOperationType CubeRoot = new CubeRootType();
        public static readonly SelfOperationType Cosine = new CosineType();
        public static readonly SelfOperationType Sine = new SineType();
        public static readonly SelfOperationType Tangent = new TangentType();
        public static readonly SelfOperationType CosineHyperbolic = new CosineHyperbolicType();
        public static readonly SelfOperationType SineHyperbolic = new SineHyperbolicType();
        public static readonly SelfOperationType TangentHyperbolic = new TangentHyperbolicType();
        public static readonly SelfOperationType Radian = new RadianType();

        static SelfOperationType()
        {
            Type type = typeof(SelfOperationType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            SelfOperationType[] allSelfOperations = new SelfOperationType[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var selfOperationType = (SelfOperationType)fields[i].GetValue(null)!;
                if (allSelfOperations[selfOperationType.Value] != null) throw new Exception("There are fields with duplicate int values");

                allSelfOperations[selfOperationType.Value] = selfOperationType;
            }
            _allSelfOperations = allSelfOperations;
        }

        private static readonly ReadOnlyMemory<SelfOperationType> _allSelfOperations;

        protected abstract int Value { get; }

        internal virtual int AdditionalCheck => 0;

        internal abstract string StringRepresentation { get; }

        internal abstract char CharRepresentation { get; }

        internal abstract double Operate(double number);

        internal static SelfOperationType Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out SelfOperationType selfOperationType)) return selfOperationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static SelfOperationType Parse(char character)
        {
            if (TryParse(character, out SelfOperationType selfOperationType)) return selfOperationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static bool TryParse(ReadOnlySpan<char> span, out SelfOperationType operationType)
        {
            operationType = Factorial;
            if (span.Length == 1) return TryParse(span[0], out operationType);

            ReadOnlySpan<SelfOperationType> reference = _allSelfOperations.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        internal static bool TryParse(char character, out SelfOperationType operationType)
        {
            operationType = Factorial;
            if (character == '\0') return false;

            ReadOnlySpan<SelfOperationType> reference = _allSelfOperations.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        internal static SelfOperationType GetSelfOperation(int value)
        {
            return _allSelfOperations.Span[value];
        }
    }
}