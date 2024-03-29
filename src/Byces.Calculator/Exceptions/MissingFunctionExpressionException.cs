﻿using Byces.Calculator.Enums;

namespace Byces.Calculator.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the expression contains a function separator in a non-function location.
    /// </summary>
    public sealed class MissingFunctionExpressionException : ExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingFunctionExpressionException"/> class.
        /// </summary>
        public MissingFunctionExpressionException() : base("The provided expression contains a function separator in a place that has no function") { }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        public override ResultError ErrorType => ResultError.MissingFunction;
    }
}