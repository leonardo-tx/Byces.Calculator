using BenchmarkDotNet.Attributes;

namespace Byces.Calculator.Tests.Benchmarks
{
    [MemoryDiagnoser]
    public class CalculatorBenchmark
    {
        [Benchmark]
        public double Simple()
        {
            return MathResultBuilder.GetMathResult("2 + 5 * 6").Result;
        }

        [Benchmark]
        public double Complex()
        {
            return MathResultBuilder.GetMathResult("2 ^ 2 + (4 + 5 * (2 √ 9))").Result;
        }

        [Benchmark]
        public double Heavy()
        {
            return MathResultBuilder.GetMathResult("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").Result;
        }

        [Benchmark]
        public double HeavyNoWhiteSpace()
        {
            return MathResultBuilder.GetMathResult("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))").Result;
        }

        [Benchmark]
        public double ManyParentheses()
        {
            return MathResultBuilder.GetMathResult("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").Result;
        }

        [Benchmark]
        public double Factorial()
        {
            return MathResultBuilder.GetMathResult("fact2 + (fact2 + fact(fact2 + 2))").Result;
        }

        [Benchmark]
        public double SquareRootString()
        {
            return MathResultBuilder.GetMathResult("SQRT9").Result;
        }

        [Benchmark]
        public double SquareRootChar()
        {
            return MathResultBuilder.GetMathResult("√9").Result;
        }
    }
}