using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CalculateAdditionExpressions()
        {
            Validate("3 + 2", 5);
            Validate("1024+1024+1024+1024", 4096);
        }

        [TestMethod]
        public void CalculateSubtractionExpressions()
        {
            Validate("12 - 5", 7);
            Validate("5-12-24-2", -33);
        }

        [TestMethod]
        public void CalculateMultiplicationExpressions()
        {
            Validate("2 * 4 * 1", 8);
            Validate("2*2*2*2*2", 32);
        }

        [TestMethod]
        public void CalculateDivisionExpressions()
        {
            Validate("5 / 2", 2.5);
            Validate("20/2/2", 5);
        }

        public void CalculatePowExpressions()
        {
            Validate("2 ^ 32", 4_294_967_296);
            Validate("3^2^5", 59_049);
        }

        public void CalculateRootExpressions()
        {
            Validate("2 RT 9", 3);
            Validate("10RT100RT9", 3);
        }

        [TestMethod]
        public void CalculateExpressionsWithMultipleOperators()
        {
            Validate("2 + 5 - 3", 4);
            Validate("6+8*3", 30);
        }

        [TestMethod]
        public void CalculateExpressionsWithNegativeNumbers()
        {
            Validate("-4 + -5 - -3", -6);
            Validate("6--8*-3", -18);
        }

        [TestMethod]
        public void CalculateSingleNumber()
        {
            Validate("4", 4);
            Validate("  5 ", 5);
            Validate("( (8))", 8);
        }

        private static void Validate(string expressionAsString, double expectedValue)
        {
            var expression = new ExpressionBuilder().WithExpression(expressionAsString).Build();
            if (!expression.IsValid) Assert.Fail(expression.ErrorMessage);

            Assert.AreEqual(expectedValue, Calculator.Calculate(expression));
        }
    }
}