using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions.Items
{
    internal abstract class OperatorItem : ExpressionItem<OperatorItem>
    {
        internal abstract OperatorPriority Priority { get; }

        internal abstract Variable Operate(Variable left, Variable right);
    }
}