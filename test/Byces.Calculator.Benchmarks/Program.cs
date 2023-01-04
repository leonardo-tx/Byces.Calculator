using BenchmarkDotNet.Running;
using System;

namespace Byces.Calculator.Benchmarks;

public sealed class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}