﻿using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Sine : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "SIN";

        public override Value Operate(Value value)
        {
            double result = Math.Sin(value.Number);
            return Math.Round(result, 15);
        }
    }
}