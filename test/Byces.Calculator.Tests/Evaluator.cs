using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    internal static class Evaluator
    {
        private readonly static ICalculator _calculator = new CalculatorBuilder().WithResultPool(false).Build();

        internal static void Validate(string expressionAsString, double expectedValue)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateApproximately(string expressionAsString, double expectedValue, double delta = 1E-4)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);
        }

        internal static void ValidateException(string expressionAsString, ResultErrorType expectedValue)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            Assert.AreEqual(expectedValue, result.ErrorType);
        }
    }
}