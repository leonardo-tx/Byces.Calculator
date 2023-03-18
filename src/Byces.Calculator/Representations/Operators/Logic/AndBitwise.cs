using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Logic
{
    internal sealed class AndBitwise : OperatorRepresentation
    {
        public override char CharRepresentation => '&';
        internal override OperatorPriority Priority => OperatorPriority.AndBitwise;

        internal override Variable Operate(Variable left, Variable right)
        {
            return left.Type switch
            {
                VariableType.Boolean => left.Boolean & right.Boolean,
                VariableType.Number => left.Long & right.Long,
                _ => throw new InvalidArgumentExpressionException(),
            };
        }
    }
}