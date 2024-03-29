﻿using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the given expression is incomplete.
    /// </summary>
    public sealed class IncompleteExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncompleteExpressionException"/> class.
        /// </summary>
        public IncompleteExpressionException(): base("The provided expression is not complete.") { }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public override ResultError ErrorType => ResultError.Incomplete;
    }
}