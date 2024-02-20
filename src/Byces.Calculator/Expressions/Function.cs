using Byces.Calculator.Exceptions;
using System;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Function
    {
        internal Function(int variableIndex, FunctionItem function, int priority)
        {
            VariableIndex = variableIndex;
            Value = function;
            Priority = priority;
        }

        internal int VariableIndex { get; }

        internal FunctionItem Value { get; }

        internal int Priority { get; }

        internal Variable Operate(ReadOnlySpan<Variable> variables)
        {
            if (Value.ParametersMin > variables.Length) throw new InvalidArgumentExpressionException();
            if (Value.ParametersMax > -1 && Value.ParametersMax < variables.Length) throw new InvalidArgumentExpressionException();
            
            return Value.Operate(variables);
        }
    }
}