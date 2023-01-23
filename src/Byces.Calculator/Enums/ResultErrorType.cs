namespace Byces.Calculator.Enums
{
    /// <summary>
    /// Specifies the error type of a <see cref="MathResult"/> instance.
    /// </summary>
    public enum ResultErrorType
    {
        /// <summary>
        /// Represents an expression with no errors.
        /// </summary>
        None,
        /// <summary>
        /// Represents an internal library error.
        /// </summary>
        Internal,
        /// <summary>
        /// Represents an incomplete expression error.
        /// </summary>
        Incomplete,
        /// <summary>
        /// Represents an unknown number expression error.
        /// </summary>
        UnknownNumber,
        /// <summary>
        /// Represents a misplaced parentheses expression error.
        /// </summary>
        MisplacedParentheses,
        /// <summary>
        /// Represents a missing parentheses expression error.
        /// </summary>
        MissingParentheses,
        /// <summary>
        /// Represents an error from an invalid calculation attempt.
        /// </summary>
        Arithmetic
    }
}