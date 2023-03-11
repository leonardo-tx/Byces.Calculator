using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions.Logic
{
    internal sealed class Not : FunctionRepresentation
    {
        public override char CharRepresentation => '!';

        public override Value Operate(Value value) => !value.Boolean;
    }
}