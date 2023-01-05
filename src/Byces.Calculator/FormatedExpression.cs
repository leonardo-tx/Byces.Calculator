using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator
{
    public readonly struct FormatedExpression
    {
        internal FormatedExpression(Content content, bool isValid, string? errorMessage = null)
        {
            Content = content;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        internal FormatedExpression(string errorMessage)
        {
            Content = Content.Empty;
            IsValid = false;
            ErrorMessage = errorMessage;
        }

        public static readonly FormatedExpression Empty = new FormatedExpression(Content.Empty, true, null);

        public bool IsValid { get; }

        public string? ErrorMessage { get; }

        private Content Content { get; }

        /// <summary>
        /// Processes the current instance and returns a <see cref="double"/> result.
        /// </summary>
        /// <returns>The expression result.</returns>
        public double GetResult()
        {
            if (!IsValid) throw new ArgumentException($"This {nameof(FormatedExpression)} instance is invalid, and cannot be used to calculate");

            Content.Process();
            return Content.Numbers[0].Value;
        }
    }
}