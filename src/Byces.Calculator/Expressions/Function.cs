using Byces.Calculator.Exceptions;
using Byces.Calculator.Representations;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Function
    {
        internal Function(int variableIndex, int function, int priority)
        {
            VariableIndex = variableIndex;
            Value = function;
            Priority = priority;
        }

        internal int VariableIndex { get; }

        internal int Value { get; }

        internal int Priority { get; }

        internal Variable Operate(Variable variable)
        {
            var functionRepresentation = (FunctionRepresentation)Value;
            if (functionRepresentation.ParametersMin > 1) throw new InvalidArgumentExpressionException();

            ReadOnlySpan<Variable> temp = stackalloc Variable[] { variable };
            return functionRepresentation.Operate(temp);
        }

        internal Variable Operate(ReadOnlySpan<Variable> variables)
        {
            var functionRepresentation = (FunctionRepresentation)Value;
            if (functionRepresentation.ParametersMin > variables.Length) throw new InvalidArgumentExpressionException();
            if (functionRepresentation.ParametersMax > -1 && functionRepresentation.ParametersMax < variables.Length) throw new InvalidArgumentExpressionException();
            
            return functionRepresentation.Operate(variables);
        }
    }
}