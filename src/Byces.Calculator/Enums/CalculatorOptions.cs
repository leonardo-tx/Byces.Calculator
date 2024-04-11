using System;

namespace Byces.Calculator.Enums
{
    /// <summary>
    /// Specifies the options to be enabled in a <see cref="CalculatorBuilder" /> instance.
    /// </summary>
    [Flags]
    public enum CalculatorOptions
    {
        /// <summary>
        /// The default experience. No options enabled.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Removes the whitespace checker. Improves performance slightly.
        /// </summary>
        RemoveWhitespaceChecker = 1,
        /// <summary>
        /// Will activate the cache of already solved expressions, making it not necessary to reparse the string.
        /// Improves performance for repeated expressions, but increases memory consumption.
        /// </summary>
        CacheExpressions = 2,
        /// <summary>
        /// Removes all the default functions provided by default.
        /// </summary>
        RemoveDefaultFunctions = 4,
        /// <summary>
        /// Removes all the default variables provided by default.
        /// </summary>
        RemoveDefaultVariables = 8,
        /// <summary>
        /// Removes all the default functions and variables provided by default.
        /// </summary>
        RemoveAllDefaultFunctionsAndVariables = 12
    }
}