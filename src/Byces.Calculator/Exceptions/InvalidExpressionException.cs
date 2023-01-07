using System;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when is used an invalid <see cref="MathExpression"/>.
    /// </summary>
    public class InvalidExpressionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionException"/> class.
        /// </summary>
        public InvalidExpressionException(): base($"This {nameof(MathExpression)} instance is invalid, and cannot be used to calculate") {}

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionException"/> class with a specified error message.
        /// </summary>
        public InvalidExpressionException(string? message): base(message) {}
    }
}