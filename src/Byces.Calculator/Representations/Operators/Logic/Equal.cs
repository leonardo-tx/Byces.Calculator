using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Operators.Logic
{
    internal sealed class Equal : OperatorRepresentation
    {
        public override string StringRepresentation => "==";
        internal override OperatorPriority Priority => OperatorPriority.Equality;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (left.Type != right.Type) return false;

            return left.Type switch
            {
                VariableType.Boolean => left.Boolean == right.Boolean,
                VariableType.Number => left.Double == right.Double,
                _ => throw new NotImplementedException()
            };
        }
    }
}