using BenchmarkDotNet.Attributes;
using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;

namespace Byces.Calculator.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class CalculatorBenchmark
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
            ExecuteDoubleCalculationMillionTimes("3");
        }
        
        [Benchmark]
        public void Random()
        {
            ExecuteDoubleCalculationMillionTimes("random(1; 10)");
        }

        [Benchmark]
        public void SimpleCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("2 + 5 * 6");
        }

        [Benchmark]
        public void EulerPlusEulerPlusEuler()
        {
            ExecuteDoubleCalculationMillionTimes("EULER + EULER + EULER");
        }

        [Benchmark]
        public void ComplexCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("2 ^ 2 + (4 + 5 * (2 √ 9))");
        }

        [Benchmark]
        public void HeavyCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("(2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9)))");
        }

        [Benchmark]
        public void HeavyCalculationNoWhiteSpace()
        {
            ExecuteDoubleCalculationMillionTimes("(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))");
        }

        [Benchmark]
        public void ManyParenthesesCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
        }

        [Benchmark]
        public void FactorialCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("fact2 + (fact2 + fact(fact2 + 2))");
        }

        [Benchmark]
        public void SquareRootStringCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("SQRT9");
        }

        [Benchmark]
        public void SquareRootCharCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("√9");
        }

        [Benchmark]
        public void AddFunctionCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("ADD(1;2;3)");
        }

        [Benchmark]
        public void AddOperationCalculation()
        {
            ExecuteDoubleCalculationMillionTimes("1+2+3");
        }

        private void ExecuteDoubleCalculationMillionTimes(string expression)
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                _calculator.GetDoubleResult(expression);
            }
        }
    }
}
