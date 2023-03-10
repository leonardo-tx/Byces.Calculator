using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    internal static class Evaluator
    {
        private readonly static ICalculator _calculator = new CalculatorBuilder().Build();

        internal static void ValidateNumber(string expressionAsString, double expectedValue)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateNumberApproximately(string expressionAsString, double expectedValue, double delta = 1E-4)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);
        }

        internal static void ValidateBoolean(string expressionAsString, bool expectedValue)
        {
            var result = _calculator.GetBooleanResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateException(string expressionAsString, ResultError expectedValue)
        {
            var result = _calculator.GetDoubleResult(expressionAsString);
            Assert.AreEqual(expectedValue, result.Error);
        }
    }
}