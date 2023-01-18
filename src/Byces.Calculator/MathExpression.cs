using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator
{
    public readonly struct MathExpression
    {
        internal MathExpression(double result, bool isValid, string? errorMessage = null)
        {
            Result = result;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        internal MathExpression(string errorMessage)
        {
            Result = 0;
            IsValid = false;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Returns a valid math expression that contains only a number 0.
        /// </summary>
        /// <returns>An expression that contains only a number 0.</returns>
        public static readonly MathExpression Default = new MathExpression(0, true, null);

        /// <summary>
        /// Gets the validity of the <see cref="MathExpression"/> instance.
        /// </summary>
        /// <returns>True if the expression is valid, false otherwise.</returns>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the error message of a possible syntax problem.
        /// </summary>
        /// <returns>The generated error message, or <c>null</c> if the expression is valid.</returns>
        public string? ErrorMessage { get; }

        private double Result { get; }

        /// <summary>
        /// Processes the current instance and returns a <see cref="double"/> result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <exception cref="InvalidExpressionException"></exception>
        public double GetResult()
        {
            if (!IsValid) throw new InvalidExpressionException();
            return Result;
        }
    }
}