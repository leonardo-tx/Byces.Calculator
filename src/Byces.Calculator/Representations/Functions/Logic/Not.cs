using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Logic
{
    internal sealed class Not : FunctionRepresentation
    {
        public override char CharRepresentation => '!';
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => !variables[0].Bool;
    }
}