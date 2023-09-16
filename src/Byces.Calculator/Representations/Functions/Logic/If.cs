﻿using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Logic
{
    internal sealed class If : FunctionRepresentation
    {
        public override string StringRepresentation => "IF";
        public override int ParametersMin => 3;
        public override int ParametersMax => 3;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            return variables[0].Bool ? variables[1] : variables[2];
        }
    }
}