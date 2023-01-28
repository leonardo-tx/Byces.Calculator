using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the expression has an invalid arithmetic calculation.
    /// </summary>
    public sealed class ArithmeticExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArithmeticExpressionException"/> class.
        /// </summary>
        public ArithmeticExpressionException() : base("Attempted an invalid operation.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArithmeticExpressionException"/> class with a specified error message.
        /// </summary>
        public ArithmeticExpressionException(string message) : base(message) { }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public override ResultErrorType ErrorType => ResultErrorType.Arithmetic;
    }
}