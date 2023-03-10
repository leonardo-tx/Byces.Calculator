using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class Modulus : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Number;
        public override string StringRepresentation => "MOD";
        public override char CharRepresentation => '%';
        internal override OperatorPriority Priority => OperatorPriority.Second;

        internal override Value Operate(Value firstValue, Value secondValue)
        {
            if (secondValue.Number == 0) return firstValue.Number;
            if (firstValue.Number < 0 && secondValue.Number > 0)
            {
                double result = secondValue.Number - firstValue.Number * -1 % secondValue.Number;
                if (result == secondValue.Number) return 0;
                
                return result;
            }
            if (firstValue.Number > 0 && secondValue.Number < 0)
            {
                double result = secondValue.Number - firstValue.Number % secondValue.Number * -1;
                if (result == secondValue.Number) return 0;

                return result;
            }
            return firstValue.Number % secondValue.Number;
        }
    }
}