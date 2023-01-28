using System;
using System.Reflection;
using Byces.Calculator.Enums.Operations;

namespace Byces.Calculator.Enums
{
    internal abstract class OperationType
    {
        public static implicit operator int(OperationType operationType) => operationType.Value;

        public static readonly OperationType Add = new Add();
        public static readonly OperationType Subtract = new Subtract();
        public static readonly OperationType Multiply = new Multiply();
        public static readonly OperationType Divide = new Divide();
        public static readonly OperationType Modulus = new Modulus();
        public static readonly OperationType Power = new Power();
        public static readonly OperationType Root = new Root();
        public static readonly OperationType Logarithm = new Logarithm();

        static OperationType()
        {
            Type type = typeof(OperationType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            OperationType[] allOperations = new OperationType[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var operationType = (OperationType)fields[i].GetValue(null)!;
                if (allOperations[operationType.Value] != null) throw new Exception("There are fields with duplicate int values");

                allOperations[operationType.Value] = operationType;
            }
            _allOperations = allOperations;
        }

        private static readonly ReadOnlyMemory<OperationType> _allOperations;

        protected abstract int Value { get; }

        internal abstract string StringRepresentation { get; }

        internal abstract char CharRepresentation { get; }

        internal abstract OperationPriorityType Priority { get; }

        internal abstract double Operate(double firstNumber, double secondNumber);

        internal static OperationType Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out OperationType operationType)) return operationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static OperationType Parse(char character)
        {
            if (TryParse(character, out OperationType operationType)) return operationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static bool TryParse(ReadOnlySpan<char> span, out OperationType operationType)
        {
            operationType = Add;
            if (span.Length == 1) return TryParse(span[0], out operationType);

            ReadOnlySpan<OperationType> reference = _allOperations.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        internal static bool TryParse(char character, out OperationType operationType)
        {
            operationType = Add;
            if (character == '\0') return false;

            ReadOnlySpan<OperationType> reference = _allOperations.Span;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        internal static OperationType GetOperation(int value)
        {
            return _allOperations.Span[value];
        }
    }
}