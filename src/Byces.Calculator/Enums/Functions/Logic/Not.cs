using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Functions.Logic
{
    internal sealed class Not : FunctionRepresentation
    {
        public override char CharRepresentation => '!';
        public override int ParametersMax => 1;

        public override Value Operate(ReadOnlySpan<Value> values) => !values[0].Boolean;
    }
}