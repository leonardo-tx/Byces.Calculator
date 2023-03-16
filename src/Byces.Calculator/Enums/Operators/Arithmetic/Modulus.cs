using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums.Operators.Arithmetic
{
    internal sealed class Modulus : OperatorRepresentation
    {
        public override string StringRepresentation => "MOD";
        public override char CharRepresentation => '%';
        internal override OperatorPriority Priority => OperatorPriority.Multiplicative;

        internal override Value Operate(Value left, Value right)
        {
            if (right.Number == 0) return left.Number;
            if (left.Number < 0 && right.Number > 0)
            {
                double result = right.Number - left.Number * -1 % right.Number;
                if (result == right.Number) return 0;

                return result;
            }
            if (left.Number > 0 && right.Number < 0)
            {
                double result = right.Number - left.Number % right.Number * -1;
                if (result == right.Number) return 0;

                return result;
            }
            return left.Number % right.Number;
        }
    }
}