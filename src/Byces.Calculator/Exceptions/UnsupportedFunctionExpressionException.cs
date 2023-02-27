using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the expression contains an unsupported function operation.
    /// </summary>
    public sealed class UnsupportedFunctionExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedFunctionExpressionException"/> class.
        /// </summary>
        public UnsupportedFunctionExpressionException() : base("Unsupported function operation attempted.") { }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public override ResultErrorType ErrorType => ResultErrorType.UnsupportedFunction;
    }
}