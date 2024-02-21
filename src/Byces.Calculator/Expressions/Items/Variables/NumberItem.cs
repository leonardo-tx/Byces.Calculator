using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items.Variables
{
    /// <summary>
    /// The class that represents a number on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class NumberItem : VariableItem
    {
        internal override VariableType VariableType => VariableType.Number;

        internal override Variable GetVariable() => GetValue();

        /// <summary>
        /// The method that will be called when this number is present in the expression.
        /// </summary>
        /// <returns>The number.</returns>
        public abstract double GetValue();
    }
}