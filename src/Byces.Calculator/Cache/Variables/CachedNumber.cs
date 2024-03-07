using Byces.Calculator.Expressions;

namespace Byces.Calculator.Cache.Variables
{
    internal sealed class CachedNumber : CachedVariable
    {
        public CachedNumber(double number)
        {
            _number = number;
        }

        private readonly double _number;
        
        public override Variable GetVariable()
        {
            return _number;
        }
    }
}