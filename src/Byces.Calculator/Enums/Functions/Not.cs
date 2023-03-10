using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Not : FunctionRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override char CharRepresentation => '!';

        public override Value Operate(Value value) => !value.Boolean;
    }
}