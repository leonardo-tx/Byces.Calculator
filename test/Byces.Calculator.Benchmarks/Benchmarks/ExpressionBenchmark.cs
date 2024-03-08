using BenchmarkDotNet.Attributes;
using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;

namespace Byces.Calculator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class ExpressionBenchmark
{
#pragma warning disable CS8618
    private ICalculator _calculator;
#pragma warning restore CS8618

    [Params(CalculatorOptions.Default, CalculatorOptions.CacheExpressions)]
    public CalculatorOptions Options;
        
    [GlobalSetup]
    public void Setup()
    {
        _calculator = new CalculatorBuilder().WithOptions(Options).Build();
    }
        
    [Benchmark]
    public void NumberOnly()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("3");
    }
        
    [Benchmark]
    public void Random()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("random(1; 10)");
    }

    [Benchmark]
    public void SimpleCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("2 + 5 * 6");
    }

    [Benchmark]
    public void EulerPlusEulerPlusEuler()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("EULER + EULER + EULER");
    }

    [Benchmark]
    public void ComplexCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("2 ^ 2 + (4 + 5 * (2 √ 9))");
    }

    [Benchmark]
    public void HeavyCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))");
    }

    [Benchmark]
    public void HeavyCalculationNoWhiteSpace()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))");
    }

    [Benchmark]
    public void ManyParenthesesCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
    }

    [Benchmark]
    public void FactorialCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("fact2 + (fact2 + fact(fact2 + 2))");
    }

    [Benchmark]
    public void SquareRootStringCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("SQRT9");
    }

    [Benchmark]
    public void SquareRootCharCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("√9");
    }

    [Benchmark]
    public void AddFunctionCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("ADD(1;2;3)");
    }

    [Benchmark]
    public void AddOperationCalculation()
    {
        ExecuteDoubleCalculationOneHundredThousandTimes("1+2+3");
    }

    private void ExecuteDoubleCalculationOneHundredThousandTimes(string expression)
    {
        for (int i = 0; i < 100_000; i++)
        {
            _calculator.GetDoubleResult(expression);
        }
    }
}
