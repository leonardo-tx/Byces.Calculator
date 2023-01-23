using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Benchmarks
{
    [TestClass]
    public class Runner
    {
        [TestMethod]
        public void RunBenchmarks()
        {
            BenchmarkRunner.Run<CalculatorBenchmark>();
        }
    }
}