using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items
{
    /// <summary>
    /// The class that represents a variable on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class VariableItem : BeforeVariableItem
    {
        /// <summary>
        /// Initializes a new <see cref="VariableItem" /> class.
        /// </summary>
        protected VariableItem()
        {
        }
        
        internal abstract VariableType VariableType { get; }

        /// <summary>
        /// The method that will be called when this variable is present in the expression.
        /// </summary>
        /// <returns>The variable.</returns>
        internal abstract Variable GetVariable();
    }
}