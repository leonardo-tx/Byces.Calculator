using BenchmarkDotNet.Attributes;

namespace Byces.Calculator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class CalculatorBenchmark
{
#pragma warning disable CS8618
    private CalculatorBuilder _calculatorBuilder;
#pragma warning restore CS8618
    
    [GlobalSetup]
    public void Setup()
    {
        _calculatorBuilder = new CalculatorBuilder();
    }
    
    [Benchmark]
    public Calculator CreateCalculatorInstance()
    {
        return _calculatorBuilder.Build();
    }
}