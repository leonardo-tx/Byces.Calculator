﻿using BenchmarkDotNet.Attributes;

namespace Byces.Calculator.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CalculatorBenchmark
    {
        [Benchmark]
        public double SimpleCalculation()
        {
            return MathResultBuilder.GetMathResult("2 + 5 * 6").Result;
        }

        [Benchmark]
        public double SimpleCalculationThousand()
        {
            double result = 0;
            for (int i = 0; i <= 1_000; i++)
            {
                result = MathResultBuilder.GetMathResult("2 + 5 * 6").Result;
            }
            return result;
        }

        [Benchmark]
        public double SimpleCalculationMillion()
        {
            double result = 0;
            for (int i = 0; i <= 1_000_000; i++)
            {
                result = MathResultBuilder.GetMathResult("2 + 5 * 6").Result;
            }
            return result;
        }

        [Benchmark]
        public double EulerPlusEulerPlusEuler()
        {
            return MathResultBuilder.GetMathResult("EULER + EULER + EULER").Result;
        }

        [Benchmark]
        public double PartialMemoryAllocation2()
        {
            return MathResultBuilder.GetMathResult("2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2 + 2").Result;
        }

        [Benchmark]
        public double ComplexCalculation()
        {
            return MathResultBuilder.GetMathResult("2 ^ 2 + (4 + 5 * (2 √ 9))").Result;
        }

        [Benchmark]
        public double HeavyCalculation()
        {
            return MathResultBuilder.GetMathResult("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))").Result;
        }

        [Benchmark]
        public double HeavyCalculationNoWhiteSpace()
        {
            return MathResultBuilder.GetMathResult("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))").Result;
        }

        [Benchmark]
        public double ManyParenthesesCalculation()
        {
            return MathResultBuilder.GetMathResult("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))").Result;
        }

        [Benchmark]
        public double FactorialCalculation()
        {
            return MathResultBuilder.GetMathResult("fact2 + (fact2 + fact(fact2 + 2))").Result;
        }

        [Benchmark]
        public double SquareRootStringCalculation()
        {
            return MathResultBuilder.GetMathResult("SQRT9").Result;
        }

        [Benchmark]
        public double SquareRootCharCalculation()
        {
            return MathResultBuilder.GetMathResult("√9").Result;
        }
    }
}