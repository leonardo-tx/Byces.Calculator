using Byces.Calculator.Expressions;
using Byces.Calculator.Expressions.Items;
using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Cache.Variables
{
    internal sealed class CachedNumberItem : CachedItem
    {
        public CachedNumberItem(VariableItem variableItem, bool isNegative) : base(variableItem)
        {
            _isNegative = isNegative;
        }

        private readonly bool _isNegative;
        
        public override Variable GetVariable()
        {
            double number = ((NumberItem)VariableItem).GetValue();
            return _isNegative ? -number : number;
        }
    }
}