using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Byces.Calculator.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public class Benchmark
{
    [Benchmark]
    public void GetCalculatorResultSimple()
    {
        var expression = new ExpressionBuilder().WithExpression("2 + 5 * 6").Build();
        Calculator.Calculate(expression);
    }


    [Benchmark]
    public void GetCalculatorResultComplex()
    {
        var expression = new ExpressionBuilder().WithExpression("2 ^ 2 + (4 + 5 * (2 √ 9))").Build();
        Calculator.Calculate(expression);
    }

    [Benchmark]
    public void GetCalculatorResultHeavy()
    {
        var expression = new ExpressionBuilder().WithExpression("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").Build();
        Calculator.Calculate(expression);
    }

    [Benchmark]
    public void GetCalculatorResultManyParentheses()
    {
        var expression = new ExpressionBuilder().WithExpression("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").Build();
        Calculator.Calculate(expression);
    }
}