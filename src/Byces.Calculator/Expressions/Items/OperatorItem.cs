using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items
{
    /// <summary>
    /// The class that represents a operator on a <see cref="Calculator"/>.
    /// </summary>
    internal abstract class OperatorItem : ExpressionItem<OperatorItem>
    {
        /// <summary>
        /// Initializes a new <see cref="OperatorItem" /> class.
        /// </summary>
        protected OperatorItem()
        {
        }
        
        /// <summary>
        /// Indicates an operator's priority.
        /// </summary>
        internal abstract OperatorPriority Priority { get; }

        /// <summary>
        /// The method that will be called when this operator is present in the expression.
        /// </summary>
        /// <param name="left">The variable on the left side of the operator.</param>
        /// <param name="right">The variable on the right side of the operator.</param>
        /// <returns></returns>
        internal abstract Variable Operate(Variable left, Variable right);
    }
}