﻿using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class AndBitwiseItem : OperatorItem
    {
        public override char CharRepresentation => '&';
        internal override OperatorPriority Priority => OperatorPriority.AndBitwise;

        internal override Variable Operate(Variable left, Variable right)
        {
            return left.Type switch
            {
                VariableType.Boolean => left.Bool & right.Bool,
                VariableType.Number => left.Long & right.Long,
                _ => throw new InvalidArgumentExpressionException(),
            };
        }
    }
}