using System;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations
{
    internal abstract class OperatorRepresentation : ExpressionRepresentation<OperatorRepresentation>
    {
        internal abstract OperatorPriority Priority { get; }

        internal abstract Variable Operate(Variable left, Variable right);
    }
}