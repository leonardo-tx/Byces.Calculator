using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations
{
    internal abstract class VariableRepresentation : BeforeVariableRepresentation
    {
        protected VariableRepresentation()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the variable. The class {GetType().FullName} has a string representation with illegal characters.");
        }

        public abstract Variable GetValue();
    }
}