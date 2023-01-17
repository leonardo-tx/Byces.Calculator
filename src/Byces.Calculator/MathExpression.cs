using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator
{
    public readonly struct MathExpression
    {
        internal MathExpression(Content content, bool isValid, string? errorMessage = null)
        {
            Content = content;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        internal MathExpression(string errorMessage)
        {
            Content = Content.Default;
            IsValid = false;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Returns a valid math expression that contains only a number 0.
        /// </summary>
        /// <returns>An expression that contains only a number 0.</returns>
        public static readonly MathExpression Default = new MathExpression(Content.Default, true, null);

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

        private Content Content { get; }

        /// <summary>
        /// Processes the current instance and returns a <see cref="double"/> result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <exception cref="InvalidExpressionException"></exception>
        public double GetResult()
        {
            if (!IsValid) throw new InvalidExpressionException();
            if (Content.Numbers.Count == 1 && Content.Numbers[0].Operations.Count == 0) return Content.Numbers[0].Value;

            Content.Process();
            return Content.Numbers[0].Value;
        }
    }
}