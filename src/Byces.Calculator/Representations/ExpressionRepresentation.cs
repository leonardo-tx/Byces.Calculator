using Byces.Calculator.Extensions;
using System;
using System.Reflection;

namespace Byces.Calculator.Representations
{
    internal abstract class ExpressionRepresentation<T> : Representable 
        where T : ExpressionRepresentation<T>
    {
        private const int StringSizeLimit = 128;

        static ExpressionRepresentation()
        {
            Type mainType = typeof(ExpressionRepresentation<T>);
            Assembly libraryAssembly = mainType.Assembly;

            ReadOnlySpan<Type> libraryTypes = libraryAssembly.GetTypes();
            for (int i = 0; i < libraryTypes.Length; i++)
            {
                Type? baseType = libraryTypes[i].BaseType;
                if (baseType == null) continue;

                if (libraryTypes[i].IsAbstract || !baseType.IsSubclassOf(mainType)) continue;
                Activator.CreateInstance(libraryTypes[i]);
            }
        }

        protected ExpressionRepresentation()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();
            bool charIsDefault = CharRepresentation == '\0';

            if (stringIsDefault && charIsDefault)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has no representation");
            if (!charIsDefault && (char.IsWhiteSpace(CharRepresentation) || char.IsDigit(CharRepresentation) || CharRepresentation == '(' || CharRepresentation == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation with an illegal character.");
            if (!stringIsDefault && spanRepresentation.Length == 1)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with length 1.");
            if (!stringIsDefault && spanRepresentation.Length > StringSizeLimit)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation above the allowed limit of {StringSizeLimit}.");
            if (!stringIsDefault && spanRepresentation.Any(x => char.IsWhiteSpace(x) || char.IsDigit(x) || x == '(' || x == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with illegal characters.");
        }
    }
}