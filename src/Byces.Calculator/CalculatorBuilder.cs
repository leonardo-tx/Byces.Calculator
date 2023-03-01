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
            HasResultPool = true;
        }

        /// <summary>
        /// Gets or sets the use of object pooling for the results of a <see cref="Calculator"/>.
        /// </summary>
        /// <returns>The stored boolean, or <see langword="true"/> if none is set.</returns>
        public bool HasResultPool { get; set; }

        /// <summary>
        /// Sets the use of object pooling for the results.
        /// </summary>
        public CalculatorBuilder WithResultPool(bool b)
        {
            HasResultPool = b;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="Calculator"/> with the given information.
        /// </summary>
        /// <returns>The built calculator.</returns>
        public Calculator Build()
        {
            return new Calculator(HasResultPool);
        }
    }
}