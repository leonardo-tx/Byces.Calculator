using System.Collections.Concurrent;
using System.Globalization;
using Byces.Calculator.Builders;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using Microsoft.Extensions.ObjectPool;

namespace Byces.Calculator.Factories
{
    internal sealed class ResultBuilderFactory : IPooledObjectPolicy<ResultBuilder>
    {
        public ResultBuilderFactory(CalculatorOptions options, BuiltExpressions builtExpressions, CultureInfo? cultureInfo)
        {
            _options = options;
            _builtExpressions = builtExpressions;
            _cultureInfo = cultureInfo;
            _cachedExpressions = (options & CalculatorOptions.CacheExpressions) != 0 
                ? new ConcurrentDictionary<int, Content>() 
                : null!;
        }

        private readonly CalculatorOptions _options;

        private readonly BuiltExpressions _builtExpressions;
        
        private readonly ConcurrentDictionary<int, Content> _cachedExpressions;

        private readonly CultureInfo? _cultureInfo;
        
        public ResultBuilder Create()
        {
            return new ResultBuilder(_options, _builtExpressions, _cultureInfo, _cachedExpressions);
        }

        public bool Return(ResultBuilder obj)
        {
            obj.Clear();
            return true;
        }
    }
}