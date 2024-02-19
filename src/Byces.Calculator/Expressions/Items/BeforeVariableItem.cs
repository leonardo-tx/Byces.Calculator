using System;
using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items
{
    /// <summary>
    /// The class that represents a expression item before a variable on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class BeforeVariableItem : ExpressionItem<BeforeVariableItem>
    {
        /// <summary>
        /// Initializes a new <see cref="BeforeVariableItem" /> class.
        /// </summary>
        protected BeforeVariableItem()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the item. The class {GetType().FullName} has a string representation with illegal characters.");
        }
        
        /// <summary>
        /// Indicates if the variable or function has no side effects and always returns the same result with the same conditions.
        /// This property is used when the <see cref="CalculatorOptions.CacheExpressions"/> option is active.
        /// </summary>
        public virtual bool Pure => false;
    }
}