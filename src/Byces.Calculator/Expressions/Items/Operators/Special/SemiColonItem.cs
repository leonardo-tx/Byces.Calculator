using Byces.Calculator.Enums;
using System;

namespace Byces.Calculator.Expressions.Items.Operators.Special
{
    internal sealed class SemiColonItem : OperatorItem
    {
        public SemiColonItem(): base(";")
        {
        }
        
        internal override OperatorPriority Priority => OperatorPriority.FunctionSeparator;

        internal override Variable Operate(Variable left, Variable right) => throw new NotSupportedException();
    }
}