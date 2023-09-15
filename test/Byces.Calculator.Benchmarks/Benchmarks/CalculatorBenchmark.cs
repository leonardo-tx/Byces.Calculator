using BenchmarkDotNet.Attributes;
using Byces.Calculator.Interfaces;

namespace Byces.Calculator.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class CalculatorBenchmark
    {
#pragma warning disable CS8618
        private ICalculator _calculator;
#pragma warning restore CS8618

        [GlobalSetup]
        public void Setup()
        {
            _calculator = new CalculatorBuilder().Build();
        }

        [Benchmark]
        public void SimpleCalculation()
        {
            _calculator.GetDoubleResult("2 + 5 * 6");
        }

        [Benchmark]
        public void SimpleCalculationThousand()
        {
            for (int i = 0; i <= 1_000; i++)
            {
                _calculator.GetDoubleResult("2 + 5 * 6");
            }
        }

        [Benchmark]
        public void SimpleCalculationMillion()
        {
            for (int i = 0; i <= 1_000_000; i++)
            {
                _calculator.GetDoubleResult("2 + 5 * 6");
            }
        }

        [Benchmark]
        public void EulerPlusEulerPlusEuler()
        {
            _calculator.GetDoubleResult("EULER + EULER + EULER");
        }

        [Benchmark]
        public void ComplexCalculation()
        {
            _calculator.GetDoubleResult("2 ^ 2 + (4 + 5 * (2 √ 9))");
        }

        [Benchmark]
        public void HeavyCalculation()
        {
            _calculator.GetDoubleResult("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))");
        }

        [Benchmark]
        public void HeavyCalculationNoWhiteSpace()
        {
            _calculator.GetDoubleResult("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))");
        }

        [Benchmark]
        public void ManyParenthesesCalculation()
        {
            _calculator.GetDoubleResult("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
        }

        [Benchmark]
        public void FactorialCalculation()
        {
            _calculator.GetDoubleResult("fact2 + (fact2 + fact(fact2 + 2))");
        }

        [Benchmark]
        public void SquareRootStringCalculation()
        {
            _calculator.GetDoubleResult("SQRT9");
        }

        [Benchmark]
        public void SquareRootCharCalculation()
        {
            _calculator.GetDoubleResult("√9");
        }

        [Benchmark]
        public void AddFunctionCalculation()
        {
            _calculator.GetDoubleResult("ADD(1;2;3)");
        }

        [Benchmark]
        public void AddOperationCalculation()
        {
            _calculator.GetDoubleResult("1+2+3");
        }
    }
}