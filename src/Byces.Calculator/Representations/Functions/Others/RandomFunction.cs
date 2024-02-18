using System;
using Byces.Calculator.Expressions;
using Microsoft.Extensions.ObjectPool;

namespace Byces.Calculator.Representations.Functions.Others
{
    internal sealed class RandomFunction : FunctionRepresentation
    {
        public override string StringRepresentation => "RANDOM";

        public override int ParametersMin => 1;

        public override int ParametersMax => 2;

        private static readonly ObjectPool<Random> RandomPool = ObjectPool.Create<Random>();
        
        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            Random randomInstance = RandomPool.Get();
            try
            {
                return variables.Length == 1
#if NET6_0_OR_GREATER
                    ? randomInstance.NextInt64(variables[0].Long)
                    : randomInstance.NextInt64(variables[0].Long, variables[1].Long);
#else
                    ? randomInstance.Next(variables[0].Int)
                    : randomInstance.Next(variables[0].Int, variables[1].Int);
#endif
            }
            finally
            {
                RandomPool.Return(randomInstance);
            }
        }
    }
}