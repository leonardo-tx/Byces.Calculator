using BenchmarkDotNet.Running;
using Byces.Calculator.Benchmarks.Benchmarks;

namespace Byces.Calculator.Benchmarks
{
    public sealed class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<CalculatorBenchmark>();
        }
    }
}