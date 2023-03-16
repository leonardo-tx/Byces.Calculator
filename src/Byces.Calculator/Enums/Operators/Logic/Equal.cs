using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Logic
{
    internal sealed class Equal : OperatorRepresentation
    {
        public override string StringRepresentation => "==";
        internal override OperatorPriority Priority => OperatorPriority.Equality;

        internal override Value Operate(Value left, Value right)
        {
            if (left.ResultType != right.ResultType) return false;

            return left.ResultType switch
            {
                ResultType.Boolean => left.Boolean == right.Boolean,
                ResultType.Number => left.Number == right.Number,
                _ => throw new NotImplementedException()
            };
        }
    }
}