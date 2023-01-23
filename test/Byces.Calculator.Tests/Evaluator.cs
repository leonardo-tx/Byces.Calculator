using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    internal static class Evaluator
    {
        internal static void Validate(string expressionAsString, double expectedValue)
        {
            var result = MathResultBuilder.GetMathResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
        }

        internal static void ValidateApproximately(string expressionAsString, double expectedValue, double delta = 1E-4)
        {
            var result = MathResultBuilder.GetMathResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);
        }

        internal static void ValidateException(string expressionAsString, ResultErrorType expectedValue)
        {
            var result = MathResultBuilder.GetMathResult(expressionAsString);
            Assert.AreEqual(expectedValue, result.ErrorType);
        }
    }
}