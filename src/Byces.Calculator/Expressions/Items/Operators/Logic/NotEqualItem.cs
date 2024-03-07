using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Logic
{
    internal sealed class NotEqualItem : OperatorItem
    {
        public NotEqualItem(): base("!=")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.Equality;

        internal override Variable Operate(Variable left, Variable right)
        {
            if (left.Type != right.Type) return true;

            return left.Type switch
            {
                VariableType.Boolean => left.Bool != right.Bool,
                VariableType.Number => left.Double != right.Double,
                _ => throw new NotSupportedException(),
            };
        }
    }
}