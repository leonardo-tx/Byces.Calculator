namespace Byces.Calculator.Representations
{
    internal abstract class BeforeVariableRepresentation : ExpressionRepresentation<BeforeVariableRepresentation>
    {
        public virtual bool Pure => false;
    }
}