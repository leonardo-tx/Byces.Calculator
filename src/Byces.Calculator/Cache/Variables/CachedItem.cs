using Byces.Calculator.Expressions;
using Byces.Calculator.Expressions.Items;

namespace Byces.Calculator.Cache.Variables
{
    internal class CachedItem : CachedVariable
    {
        public CachedItem(VariableItem variableItem)
        {
            VariableItem = variableItem;
        }

        protected readonly VariableItem VariableItem;
        
        public override Variable GetVariable()
        {
            return VariableItem.GetVariable();
        }
    }
}