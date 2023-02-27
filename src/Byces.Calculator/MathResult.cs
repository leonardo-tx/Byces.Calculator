using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator
{
    /// <summary>
    /// Represents the result of a mathematical expression.
    /// </summary>
    public readonly struct MathResult<T>
    {
        internal MathResult(T result, bool isValid, string? errorMessage = null)
        {
            Result = result;
            IsValid = isValid;
            ErrorMessage = errorMessage;
            ErrorType = ResultErrorType.None;
        }

        internal MathResult(Exception exception, T result)
        {
            Result = result;
            IsValid = false;
            ErrorMessage = exception.Message;
            if (exception is ExpressionException expressionException)
            {
                ErrorType = expressionException.ErrorType;
                return;
            }
            ErrorType = ResultErrorType.Internal;
        }

        /// <summary>
        /// Gets the validity of the <see cref="MathResult{T}"/> instance.
        /// </summary>
        /// <returns>True if the expression is valid, false otherwise.</returns>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the error message of a possible syntax problem.
        /// </summary>
        /// <returns>The generated error message, or <c>null</c> if the expression is valid.</returns>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Gets the expression error type.
        /// </summary>
        /// <returns>The type of error.</returns>
        public ResultErrorType ErrorType { get; }

        /// <summary>
        /// Gets the expression result.
        /// </summary>
        /// <returns>The expression result.</returns>
        public T Result { get; }
    }
}