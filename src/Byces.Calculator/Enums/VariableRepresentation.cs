using Byces.Calculator.Expressions;

namespace Byces.Calculator.Enums
{
    internal abstract class VariableRepresentation : ExpressionRepresentation<VariableRepresentation>
    {
        protected VariableRepresentation() : base(ExpressionConflict.Variable | ExpressionConflict.Function) { }

        internal abstract Value GetValue();

        internal override ExpressionConflict RepresentationConflict => ExpressionConflict.Variable;
    }
}