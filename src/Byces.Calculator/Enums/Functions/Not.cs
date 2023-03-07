using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Functions
{
    internal sealed class Not : FunctionType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override char CharRepresentation => '!';

        public override Value Operate(Value value) => !value.Boolean;
    }
}