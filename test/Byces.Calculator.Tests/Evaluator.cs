using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    internal static class Evaluator
    {
        private static readonly ICalculator Calculator = new CalculatorBuilder().Build();

        internal static void ValidateNumber(string expressionAsString, double expectedValue)
        {
            MathResult<double> result = Calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateNumberApproximately(string expressionAsString, double expectedValue, double delta = 1E-4)
        {
            MathResult<double> result = Calculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);
        }

        internal static void ValidateBoolean(string expressionAsString, bool expectedValue)
        {
            MathResult<bool> result = Calculator.GetBooleanResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateException(string expressionAsString, ResultError expectedValue)
        {
            MathResult<double> result = Calculator.GetDoubleResult(expressionAsString);
            Assert.AreEqual(expectedValue, result.Error);
        }
    }
}