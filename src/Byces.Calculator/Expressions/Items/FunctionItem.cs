﻿using System;

namespace Byces.Calculator.Expressions.Items
{
    internal abstract class FunctionItem : BeforeVariableItem
    {
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

        public virtual int ParametersMin => 1;

        public virtual int ParametersMax => -1;

        public abstract Variable Operate(ReadOnlySpan<Variable> variables);
    }
}