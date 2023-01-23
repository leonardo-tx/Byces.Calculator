using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the given expression contains misplaced parentheses.
    /// </summary>
    public sealed class MisplacedParenthesesExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MisplacedParenthesesExpressionException"/> class.
        /// </summary>
        public MisplacedParenthesesExpressionException(): base("Provided expression has misplaced parentheses.") { }

        public override ResultErrorType ErrorType => ResultErrorType.MisplacedParentheses;
    }
}