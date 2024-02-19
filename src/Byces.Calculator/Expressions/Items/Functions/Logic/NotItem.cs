using System;

namespace Byces.Calculator.Expressions.Items.Functions.Logic
{
    internal sealed class NotItem : FunctionItem
    {
        public override char CharRepresentation => '!';
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => !variables[0].Bool;
    }
}