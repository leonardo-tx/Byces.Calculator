using Byces.Calculator.Exceptions;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operators.Logic
{
    internal sealed class OrBitwise : OperatorRepresentation
    {
        public override char CharRepresentation => '|';
        internal override OperatorPriority Priority => OperatorPriority.OrBitwise;

        internal override Value Operate(Value left, Value right)
        {
            return left.ResultType switch
            {
                ResultType.Boolean => left.Boolean | right.Boolean,
                ResultType.Number => GetNumberBitwise(left.Number, right.Number),
                _ => throw new NotImplementedException()
            };
        }

        private static double GetNumberBitwise(double left, double right)
        {
            double difference1 = left - (long)left;
            double difference2 = right - (long)right;
            if (difference1 != 0 || difference2 != 0) throw new InvalidArgumentExpressionException();

            return (long)left | (long)right;
        }
    }
}