using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when is used an invalid argument parameter, in a function or operator.
    /// </summary>
    public sealed class InvalidArgumentExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentExpressionException"/> class.
        /// </summary>
        public InvalidArgumentExpressionException(): base("Attempted invalid argument in a function or operation.") { }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public override ResultError ErrorType => ResultError.InvalidArgument;
    }
}