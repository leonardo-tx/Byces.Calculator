using System;

namespace Byces.Calculator.Interfaces
{
    /// <summary>
    /// This interface provides the math expression calculation functionality.
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Gets the number of expressions stored in the current calculator.
        /// </summary>
        public int CachedCount { get; }

        /// <summary>
        /// Releases all cache used to store expressions, if the option is enabled.
        /// </summary>
        public void FreeExpressionsCache();
        
        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string? expression);
        
        /// <summary>
        /// Gets a <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(ReadOnlySpan<char> expression);

        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(string? expression);
        
        /// <summary>
        /// Gets a <see langword="bool"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<bool> GetBooleanResult(ReadOnlySpan<char> expression);
    }
}