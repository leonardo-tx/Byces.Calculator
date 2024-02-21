using System;
using System.Globalization;
using System.Reflection;
using Byces.Calculator.Builders;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

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
        /// Gets or sets the culture of a <see cref="Calculator" />.
        /// </summary>
        /// <returns>The default culture of a <see cref="Calculator" />, or <see langword="null" /> if none is set.</returns>
        public CultureInfo? CultureInfo { get; set; }

        /// <summary>
        /// Gets or sets the options of a <see cref="Calculator" />.
        /// </summary>
        /// <returns>An enum value of the enabled options.</returns>
        public CalculatorOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the assemblies that will be used to search for operators, functions and variables when building the <see cref="Calculator" />.
        /// </summary>
        /// <returns>An array of assembly.</returns>
        public Assembly[] Assemblies { get; set; } = Array.Empty<Assembly>();

        /// <summary>
        /// Sets the culture of a <see cref="Calculator" />.
        /// </summary>
        /// <param name="cultureInfo">The culture to be set.</param>
        /// <returns>The current builder.</returns>
        public CalculatorBuilder WithCultureInfo(CultureInfo? cultureInfo)
        {
            CultureInfo = cultureInfo;
            return this;
        }
        
        /// <summary>
        /// Sets the options of a <see cref="Calculator" />.
        /// </summary>
        /// <param name="options">The options to be set.</param>
        /// <returns>The current builder.</returns>
        public CalculatorBuilder WithOptions(CalculatorOptions options)
        {
            Options = options;
            return this;
        }

        /// <summary>
        /// Sets the assemblies that will be used to search for functions and variables when building the <see cref="Calculator" />.
        /// </summary>
        /// <param name="assemblies">The assemblies to be set.</param>
        /// <returns>The current builder.</returns>
        public CalculatorBuilder WithAssemblies(params Assembly[] assemblies)
        {
            Assemblies = assemblies;
            return this;
        }

        /// <summary>
        /// Builds a new <see cref="Calculator"/> instance.
        /// </summary>
        /// <returns>The built calculator.</returns>
        public Calculator Build()
        {
            BuiltExpressions builtExpressions = new(Options, Assemblies);
            CalculatorDependencies dependencies = new(Options, builtExpressions, CultureInfo);
            
            return new Calculator(dependencies);
        }
    }
}