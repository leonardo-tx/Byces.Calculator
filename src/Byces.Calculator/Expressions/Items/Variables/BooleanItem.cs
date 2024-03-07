using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Variables
{
    /// <summary>
    /// The class that represents a boolean on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class BooleanItem : VariableItem
    {
        protected BooleanItem(params string[] stringRepresentations): base(stringRepresentations)
        {
        }
        
        internal override VariableType VariableType => VariableType.Boolean;

        internal override Variable GetVariable() => GetValue();

        /// <summary>
        /// The method that will be called when this boolean is present in the expression.
        /// </summary>
        /// <returns>The boolean.</returns>
        public abstract bool GetValue();
    }
}