using System;

namespace Byces.Calculator.Enums
{
    internal abstract class VariableRepresentation : ExpressionRepresentation<VariableRepresentation>
    {
        protected VariableRepresentation() : base(ExpressionConflict.Variable | ExpressionConflict.Function) { }

        internal virtual double GetNumber() => throw new NotSupportedException();

        internal virtual bool GetBoolean() => throw new NotSupportedException();

        internal override ExpressionConflict RepresentationConflict => ExpressionConflict.Variable;
    }
}