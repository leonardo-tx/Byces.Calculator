using Byces.Calculator.Builders;
using Byces.Calculator.Expressions;
using Microsoft.Extensions.ObjectPool;

namespace Byces.Calculator.Factories
{
    internal sealed class ResultBuilderFactory : IPooledObjectPolicy<ResultBuilder>
    {
        public ResultBuilderFactory(CalculatorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private readonly CalculatorDependencies _dependencies;
        
        public ResultBuilder Create()
        {
            return new ResultBuilder(_dependencies);
        }

        public bool Return(ResultBuilder obj)
        {
            obj.Clear();
            return true;
        }
    }
}