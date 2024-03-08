using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Variables
{
    /// <summary>
    /// The class that represents a number on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class NumberItem : VariableItem
    {
        /// <summary>
        /// Initializes a new <see cref="NumberItem" /> class.
        /// </summary>
        /// <param name="stringRepresentations">String representations for the item.</param>
        protected NumberItem(params string[] stringRepresentations): base(stringRepresentations)
        {
        }
        
        internal override VariableType VariableType => VariableType.Number;

        internal override Variable GetVariable() => GetValue();

        /// <summary>
        /// The method that will be called when this number is present in the expression.
        /// </summary>
        /// <returns>The number.</returns>
        public abstract double GetValue();
        
    }
}