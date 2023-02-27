﻿using Byces.Calculator.Enums;
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

        internal double Operate(double number) => ((FunctionType)Value).Operate(number);

        internal double Operate(ReadOnlySpan<double> numbers) => ((FunctionType)Value).Operate(numbers);
    }
}