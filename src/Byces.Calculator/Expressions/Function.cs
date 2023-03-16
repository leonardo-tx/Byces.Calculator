using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Function
    {
        internal Function(int numberIndex, int function, int priority)
        {
            NumberIndex = numberIndex;
            Value = function;
            Priority = priority;
        }

        internal int NumberIndex { get; }

        internal int Value { get; }

        internal int Priority { get; }

        internal Value Operate(Value value)
        {
            var functionRepresentation = (FunctionRepresentation)Value;
            if (functionRepresentation.ParametersMin > 1) throw new InvalidArgumentExpressionException();

            ReadOnlySpan<Value> temp = stackalloc Value[] { value };
            return functionRepresentation.Operate(temp);
        }

        internal Value Operate(ReadOnlySpan<Value> values)
        {
            var functionRepresentation = (FunctionRepresentation)Value;
            if (functionRepresentation.ParametersMin > values.Length) throw new InvalidArgumentExpressionException();
            if (functionRepresentation.ParametersMax > -1 && functionRepresentation.ParametersMax < values.Length) throw new InvalidArgumentExpressionException();
            
            return functionRepresentation.Operate(values);
        }
    }
}