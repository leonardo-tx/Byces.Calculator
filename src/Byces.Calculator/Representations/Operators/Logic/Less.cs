using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations.Operators.Logic
{
    internal sealed class Less : OperatorRepresentation
    {
        public override char CharRepresentation => '<';
        internal override OperatorPriority Priority => OperatorPriority.Relational;

        internal override Variable Operate(Variable left, Variable right) => left.Double < right.Double;
    }
}