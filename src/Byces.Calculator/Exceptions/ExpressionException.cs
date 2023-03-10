using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when there is a problem in the given expression.
    /// </summary>
    public abstract class ExpressionException : Exception
    {
        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public abstract ResultError ErrorType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionException"/> class with a specified error message.
        /// </summary>
        public ExpressionException(string message) : base(message) { }
    }
}