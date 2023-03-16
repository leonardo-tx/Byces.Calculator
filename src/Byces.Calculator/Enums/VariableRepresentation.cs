using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums
{
    internal abstract class VariableRepresentation : ExpressionRepresentation<VariableRepresentation>
    {
        protected VariableRepresentation() : base(ExpressionConflict.Variable | ExpressionConflict.Function, ExpressionConflict.Variable) { }

        public abstract Value GetValue();
    }
}