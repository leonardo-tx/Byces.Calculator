using BenchmarkDotNet.Running;
using Byces.Calculator.Benchmarks.Benchmarks;

namespace Byces.Calculator.Benchmarks
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<CalculatorBenchmark>();
        }
    }
}