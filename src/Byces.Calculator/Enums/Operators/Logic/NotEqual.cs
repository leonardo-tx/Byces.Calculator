using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Logic
{
    internal sealed class NotEqual : OperatorRepresentation
    {
        public override string StringRepresentation => "!=";
        internal override OperatorPriority Priority => OperatorPriority.Equality;

        internal override Value Operate(Value left, Value right)
        {
            if (left.ResultType != right.ResultType) return true;

            return left.ResultType switch
            {
                ResultType.Boolean => left.Boolean != right.Boolean,
                ResultType.Number => left.Number != right.Number,
                _ => throw new NotSupportedException(),
            };
        }
    }
}