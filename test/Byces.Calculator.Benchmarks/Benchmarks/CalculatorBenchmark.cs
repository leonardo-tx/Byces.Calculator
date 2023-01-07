using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Byces.Calculator.Benchmarks.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [MemoryDiagnoser]
    public class CalculatorBenchmark
    {
        [Benchmark]
        public double GetCalculatorResultSimple()
        {
            return new ExpressionBuilder().WithExpression("2 + 5 * 6").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultComplex()
        {
            return new ExpressionBuilder().WithExpression("2 ^ 2 + (4 + 5 * (2 √ 9))").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultHeavy()
        {
            return new ExpressionBuilder().WithExpression("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultManyParentheses()
        {
            return new ExpressionBuilder().WithExpression("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultFactorial()
        {
            return new ExpressionBuilder().WithExpression("2! + (2! + (2! + 2)!)").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultSquareRoot()
        {
            return new ExpressionBuilder().WithExpression("SQRT9").Build().GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultSquareRoot2()
        {
            return new ExpressionBuilder().WithExpression("√9").Build().GetResult();
        }
    }
}