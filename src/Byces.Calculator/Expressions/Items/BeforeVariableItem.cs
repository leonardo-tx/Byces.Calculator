namespace Byces.Calculator.Expressions.Items
{
    internal abstract class BeforeVariableItem : ExpressionItem<BeforeVariableItem>
    {
        public virtual bool Pure => false;
    }
}