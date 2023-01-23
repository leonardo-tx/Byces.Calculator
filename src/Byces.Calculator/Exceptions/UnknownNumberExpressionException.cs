using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the given expression contains an unknown number.
    /// </summary>
    public sealed class UnknownNumberExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownNumberExpressionException"/> class.
        /// </summary>
        public UnknownNumberExpressionException(): base("The given expression has an unknown number.") { }

        public override ResultErrorType ErrorType => ResultErrorType.UnknownNumber;
    }
}