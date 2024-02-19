using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class EqualItem : OperatorItem
    {
        public override string StringRepresentation => "==";
        internal override OperatorPriority Priority => OperatorPriority.Equality;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (left.Type != right.Type) return false;

            return left.Type switch
            {
                VariableType.Boolean => left.Bool == right.Bool,
                VariableType.Number => left.Double == right.Double,
                _ => throw new NotImplementedException()
            };
        }
    }
}