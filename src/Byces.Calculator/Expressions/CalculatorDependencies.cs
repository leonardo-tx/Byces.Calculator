using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Byces.Calculator.Builders;
using Byces.Calculator.Collections;
using Byces.Calculator.Enums;

namespace Byces.Calculator.Expressions
{
    internal sealed class CalculatorDependencies
    {
        public CalculatorDependencies(CalculatorOptions options, BuiltExpressions builtExpressions, CultureInfo? cultureInfo)
        {
            Options = options;
            BuiltExpressions = builtExpressions;
            CultureInfo = cultureInfo;
            CachedExpressions = (options & CalculatorOptions.CacheExpressions) != 0 ? new ExpressionsCache() : null;
        }
        
        internal readonly ExpressionsCache? CachedExpressions;
        
        internal readonly CalculatorOptions Options;
        
        internal readonly BuiltExpressions BuiltExpressions;

        internal readonly CultureInfo? CultureInfo;

        [MemberNotNullWhen(true, nameof(CachedExpressions))]
        public bool HasCachedExpressions()
        {
            return (Options & CalculatorOptions.CacheExpressions) != 0;
        }
        
        public bool HasWhitespaceCheck()
        {
            return (Options & CalculatorOptions.RemoveWhitespaceChecker) == 0;
        }
    }
}