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
            return ExpressionBuilder.GetMathExpression("2 + 5 * 6").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultComplex()
        {
            return ExpressionBuilder.GetMathExpression("2 ^ 2 + (4 + 5 * (2 √ 9))").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultHeavy()
        {
            return ExpressionBuilder.GetMathExpression("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultManyParentheses()
        {
            return ExpressionBuilder.GetMathExpression("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultFactorial()
        {
            return ExpressionBuilder.GetMathExpression("fact2 + (fact2 + fact(fact2 + 2))").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultSquareRoot()
        {
            return ExpressionBuilder.GetMathExpression("SQRT9").GetResult();
        }

        [Benchmark]
        public double GetCalculatorResultSquareRoot2()
        {
            return ExpressionBuilder.GetMathExpression("√9").GetResult();
        }
    }
}