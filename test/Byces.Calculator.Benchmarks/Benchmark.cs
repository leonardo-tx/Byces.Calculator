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
        new ExpressionBuilder().WithExpression("2 + 5 * 6").Build().Calculate();
    }

    [Benchmark]
    public void GetCalculatorResultComplex()
    {
        new ExpressionBuilder().WithExpression("2 ^ 2 + (4 + 5 * (2 √ 9))").Build().Calculate();
    }

    [Benchmark]
    public void GetCalculatorResultHeavy()
    {
        new ExpressionBuilder().WithExpression("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").Build().Calculate();
    }

    [Benchmark]
    public void GetCalculatorResultManyParentheses()
    {
        new ExpressionBuilder().WithExpression("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").Build().Calculate();
    }

    [Benchmark]
    public void GetCalculatorResultFactorial()
    {
        new ExpressionBuilder().WithExpression("2! + (2! + (2! + 2)!)").Build().Calculate();
    }
}