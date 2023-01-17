using Ardalis.SmartEnum;
using Byces.Calculator.Enums.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Byces.Calculator.Enums
{
    internal abstract class OperationType : SmartEnum<OperationType>
    {
        public static readonly OperationType Add = new AddType(0);
        public static readonly OperationType Subtract = new SubtractType(1);
        public static readonly OperationType Multiply = new MultiplyType(2);
        public static readonly OperationType Divide = new DivideType(3);
        public static readonly OperationType Modulus = new ModulusType(4);
        public static readonly OperationType Power = new PowerType(5);
        public static readonly OperationType Root = new RootType(6);
        public static readonly OperationType Factorial = new FactorialType(7);
        public static readonly OperationType SquareRoot = new SquareRootType(8);
        public static readonly OperationType CubeRoot = new CubeRootType(9);
        public static readonly OperationType Cosine = new CosineType(10);
        public static readonly OperationType Sine = new SineType(11);
        public static readonly OperationType Tangent = new TangentType(12);
        public static readonly OperationType CosineHyperbolic = new CosineHyperbolicType(13);
        public static readonly OperationType SineHyperbolic = new SineHyperbolicType(14);
        public static readonly OperationType TangentHyperbolic = new TangentHyperbolicType(15);

        static OperationType()
        {
            IList<OperationType> normalOperations = new List<OperationType>();
            IList<OperationType> beforeOperations = new List<OperationType>();
            IList<OperationType> afterOperations = new List<OperationType>();
            IList<char> normalOperationChars = new List<char>();
            IList<char> beforeOperationChars = new List<char>();
            IList<char> afterOperationChars = new List<char>();

            Type type = typeof(OperationType);
            ReadOnlySpan<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                var operationType = (OperationType)fields[i].GetValue(null)!;
                switch (operationType.Category) 
                { 
                    case OperationCategory.None:
                        normalOperationChars.Add(operationType.CharRepresentation);
                        normalOperations.Add(operationType);
                        break;
                    case OperationCategory.Before:
                        beforeOperationChars.Add(operationType.CharRepresentation);
                        beforeOperations.Add(operationType);
                        break;
                    case OperationCategory.After:
                        afterOperationChars.Add(operationType.CharRepresentation);
                        afterOperations.Add(operationType);
                        break;
                }
            }
            _normalOperations = normalOperations.ToArray();
            _beforeOperations = beforeOperations.ToArray();
            _afterOperations = afterOperations.ToArray();
        }

        private static readonly ReadOnlyMemory<OperationType> _normalOperations;
        private static readonly ReadOnlyMemory<OperationType> _beforeOperations;
        private static readonly ReadOnlyMemory<OperationType> _afterOperations;

        protected OperationType(string name, int value): base(name, value) { }

        internal abstract string StringRepresentation { get; }

        internal abstract char CharRepresentation { get; }

        internal abstract OperationCategory Category { get; }

        internal abstract double Operate(double firstNumber, double secondNumber);

        internal abstract double Operate(double number);

        internal static OperationType Parse(ReadOnlySpan<char> span, OperationCategory category)
        {
            if (TryParse(span, category, out OperationType operationType)) return operationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static OperationType Parse(char character, OperationCategory category)
        {
            if (TryParse(character, category, out OperationType operationType)) return operationType;
            throw new NotSupportedException("Not supported operation.");
        }

        internal static bool TryParse(ReadOnlySpan<char> span, OperationCategory category, out OperationType operationType)
        {
            operationType = Add;
            if (span.Length == 1) return TryParse(span[0], category, out operationType);

            ReadOnlySpan<OperationType> reference = GetOperationTypeReference(category);
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        internal static bool TryParse(char character, OperationCategory category, out OperationType operationType)
        {
            operationType = Add;
            if (character == '\0') return false;

            ReadOnlySpan<OperationType> reference = GetOperationTypeReference(category);
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                operationType = reference[i]; return true;
            }
            return false;
        }

        private static ReadOnlySpan<OperationType> GetOperationTypeReference(OperationCategory category)
        {
            return category switch
            {
                OperationCategory.None => _normalOperations.Span,
                OperationCategory.Before => _beforeOperations.Span,
                OperationCategory.After => _afterOperations.Span,
                _ => throw new NotImplementedException(),
            };
        }
    }
}