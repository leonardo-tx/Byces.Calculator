using System;

namespace Byces.Calculator.Expressions.Items
{
    /// <summary>
    /// The class that represents a function on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class FunctionItem : BeforeVariableItem
    {
        /// <summary>
        /// Initializes a new <see cref="FunctionItem"/> class.
        /// </summary>
        protected FunctionItem()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} has a string representation with illegal characters.");
            if (ParametersMax == 0)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} cannot have a parameter limit of 0");
            if (ParametersMin < 1)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} cannot have a minimum parameter less than 1");
            if (ParametersMax > 0 && ParametersMin > ParametersMax)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} has a maximum parameter limit '{ParametersMax}' less than the minimum '{ParametersMin}'");
        }

        /// <summary>
        /// Indicates the minimum number of parameters the function expects. Default is 1.
        /// </summary>
        public virtual int ParametersMin => 1;

        /// <summary>
        /// Indicates the maximum number of parameters the function expects. Default is -1, which means it has no limits.
        /// </summary>
        public virtual int ParametersMax => -1;

        /// <summary>
        /// The method that will be called when this function is present in the expression.
        /// </summary>
        /// <param name="variables">The function parameters.</param>
        /// <returns>The result of the function.</returns>
        public abstract Variable Operate(ReadOnlySpan<Variable> variables);
    }
}