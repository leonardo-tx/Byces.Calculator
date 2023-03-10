namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of a <see cref="Calculator"/> in a simplified way.
    /// </summary>
    public sealed class CalculatorBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="CalculatorBuilder"/> class.
        /// </summary>
        public CalculatorBuilder()
        {

        }

        /// <summary>
        /// Builds a new <see cref="Calculator"/> instance.
        /// </summary>
        /// <returns>The built calculator.</returns>
        public Calculator Build()
        {
            return new Calculator();
        }
    }
}