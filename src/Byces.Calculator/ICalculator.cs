namespace Byces.Calculator
{
    /// <summary>
    /// This interface provides the math expression calculation functionality.
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Gets the <see langword="double"/> <see cref="MathResult{T}"/>, calculating the given mathematical expression.
        /// </summary>
        /// <param name="expression">The mathematical expression.</param>
        /// <returns>The built result.</returns>
        public MathResult<double> GetDoubleResult(string expression);
    }
}