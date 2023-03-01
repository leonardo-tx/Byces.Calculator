using System;
using System.Collections.Generic;
using System.Reflection;

namespace Byces.Calculator.Enums
{
    internal abstract class FunctionType
    {
        public static implicit operator int(FunctionType functionType) => functionType.value;
        public static explicit operator FunctionType(int value) => GetFunction(value);

        static FunctionType()
        {
            Type mainType = typeof(FunctionType);
            Assembly libraryAssembly = mainType.Assembly;

            ReadOnlySpan<Type> libraryTypes = libraryAssembly.GetTypes();
            List<Type> functionTypes = new List<Type>();
            for (int i = 0; i < libraryTypes.Length; i++)
            {
                if (libraryTypes[i].IsAbstract || !libraryTypes[i].IsSubclassOf(mainType)) continue;
                functionTypes.Add(libraryTypes[i]);
            }

            int index = 0;
            _items = new FunctionType[functionTypes.Count];
            foreach (Type type in functionTypes)
            {
                var instance = (FunctionType)Activator.CreateInstance(type)!;
                instance.value = index;

                if (instance.StringRepresentation.Length > MaxStringSize) MaxStringSize = instance.StringRepresentation.Length;
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i] == null) break;
                    if (_items[i].StringRepresentation == instance.StringRepresentation) 
                        throw new Exception($"Unable to initialize the functions. The {type.FullName} class has a string representation identical to another function.");
                }

                _items[index++] = instance;
            }
        }

        private static readonly FunctionType[] _items;

        private int value;

        protected abstract string StringRepresentation { get; }

        protected abstract char CharRepresentation { get; }

        internal virtual int AdditionalCheck => 0;

        internal static int MaxStringSize { get; }

        public abstract double Operate(double number);

        public abstract double Operate(ReadOnlySpan<double> numbers);

        internal static bool TryParse(ReadOnlySpan<char> span, out FunctionType functionType)
        {
            if (span.Length == 1) return TryParse(span[0], out functionType);

            ReadOnlySpan<FunctionType> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (!span.Equals(reference[i].StringRepresentation, StringComparison.OrdinalIgnoreCase)) continue;
                functionType = reference[i]; return true;
            }
            functionType = default!; return false;
        }

        internal static bool TryParse(char character, out FunctionType functionType)
        {
            if (character == '\0') { functionType = default!; return false; }

            ReadOnlySpan<FunctionType> reference = _items;
            for (int i = 0; i < reference.Length; i++)
            {
                if (character != reference[i].CharRepresentation) continue;
                functionType = reference[i]; return true;
            }
            functionType = default!; return false;
        }

        internal static FunctionType GetFunction(int value) => _items[value];
    }
}