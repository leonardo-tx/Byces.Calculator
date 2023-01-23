using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the given expression is missing parentheses to be closed.
    /// </summary>
    public sealed class MissingParenthesesExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingParenthesesExpressionException"/> class.
        /// </summary>
        public MissingParenthesesExpressionException(): base("The provided expression is missing parentheses to be closed.") { }

        public override ResultErrorType ErrorType => ResultErrorType.MissingParentheses;
    }
}