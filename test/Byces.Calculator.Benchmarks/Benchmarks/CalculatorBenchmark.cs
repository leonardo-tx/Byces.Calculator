using BenchmarkDotNet.Attributes;
using Byces.Calculator.Interfaces;

namespace Byces.Calculator.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class CalculatorBenchmark
    {
#pragma warning disable CS8618
        private ICalculator calculator;
#pragma warning restore CS8618

        [GlobalSetup]
        public void Setup()
        {
            calculator = new CalculatorBuilder().Build();
        }

        [Benchmark]
        public void SimpleCalculation()
        {
            calculator.GetDoubleResult("2 + 5 * 6");
        }

        [Benchmark]
        public void SimpleCalculationThousand()
        {
            for (int i = 0; i <= 1_000; i++)
            {
                calculator.GetDoubleResult("2 + 5 * 6");
            }
        }

        [Benchmark]
        public void SimpleCalculationMillion()
        {
            for (int i = 0; i <= 1_000_000; i++)
            {
                calculator.GetDoubleResult("2 + 5 * 6");
            }
        }

        [Benchmark]
        public void EulerPlusEulerPlusEuler()
        {
            calculator.GetDoubleResult("EULER + EULER + EULER");
        }

        [Benchmark]
        public void ComplexCalculation()
        {
            calculator.GetDoubleResult("2 ^ 2 + (4 + 5 * (2 √ 9))");
        }

        [Benchmark]
        public void HeavyCalculation()
        {
            calculator.GetDoubleResult("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))");
        }

        [Benchmark]
        public void HeavyCalculationNoWhiteSpace()
        {
            calculator.GetDoubleResult("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))");
        }

        [Benchmark]
        public void ManyParenthesesCalculation()
        {
            calculator.GetDoubleResult("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
        }

        [Benchmark]
        public void FactorialCalculation()
        {
            calculator.GetDoubleResult("fact2 + (fact2 + fact(fact2 + 2))");
        }

        [Benchmark]
        public void SquareRootStringCalculation()
        {
            calculator.GetDoubleResult("SQRT9");
        }

        [Benchmark]
        public void SquareRootCharCalculation()
        {
            calculator.GetDoubleResult("√9");
        }

        [Benchmark]
        public void AddFunctionCalculation()
        {
            calculator.GetDoubleResult("ADD(1;2;3)");
        }

        [Benchmark]
        public void AddOperationCalculation()
        {
            calculator.GetDoubleResult("1+2+3");
        }
    }
}