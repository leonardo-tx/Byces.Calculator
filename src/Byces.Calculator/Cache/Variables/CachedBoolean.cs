using Byces.Calculator.Expressions;

namespace Byces.Calculator.Cache.Variables
{
    internal sealed class CachedBoolean : CachedVariable
    {
        public CachedBoolean(bool boolean)
        {
            _boolean = boolean;
        }

        private readonly bool _boolean;
        
        public override Variable GetVariable()
        {
            return _boolean;
        }
    }
}