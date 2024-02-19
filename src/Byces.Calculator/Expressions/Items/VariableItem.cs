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

        public abstract Variable GetValue();
    }
}