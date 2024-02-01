using Byces.Calculator.Exceptions;
using Byces.Calculator.Representations;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Function
    {
        internal Function(int variableIndex, FunctionRepresentation function, int priority)
        {
            VariableIndex = variableIndex;
            Value = function;
            Priority = priority;
        }

        internal int VariableIndex { get; }

        internal FunctionRepresentation Value { get; }

        internal int Priority { get; }

        internal Variable Operate(Variable variable)
        {
            if (Value.ParametersMin > 1) throw new InvalidArgumentExpressionException();

            ReadOnlySpan<Variable> temp = stackalloc Variable[] { variable };
            return Value.Operate(temp);
        }

        internal Variable Operate(ReadOnlySpan<Variable> variables)
        {
            if (Value.ParametersMin > variables.Length) throw new InvalidArgumentExpressionException();
            if (Value.ParametersMax > -1 && Value.ParametersMax < variables.Length) throw new InvalidArgumentExpressionException();
            
            return Value.Operate(variables);
        }
    }
}