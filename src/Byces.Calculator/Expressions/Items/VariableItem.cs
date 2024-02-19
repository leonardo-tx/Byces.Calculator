using System;

namespace Byces.Calculator.Expressions.Items
{
    internal abstract class VariableItem : BeforeVariableItem
    {
        protected VariableItem()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the variable. The class {GetType().FullName} has a string representation with illegal characters.");
        }

        public abstract Variable GetValue();
    }
}