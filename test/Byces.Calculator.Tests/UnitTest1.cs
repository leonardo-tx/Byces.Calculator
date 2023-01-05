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

        [TestMethod]
        public void CalculatePowExpressions()
        {
            Validate("2 ^ 32", 4_294_967_296);
            Validate("3^2^5", 59_049);
        }
        
        [TestMethod]
        public void CalculateRootExpressions()
        {
            Validate("√9", 3);
            Validate("2 √ 9", 3);
            ValidateApproximately("√5!", 10.9544);
            Validate("2√9√64", 4);
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

        [TestMethod]
        public void CalculateEmpty()
        {
            Validate("", 0);
            Validate("    ", 0);
        }

        [TestMethod]
        public void CalculateFactorial()
        {
            Validate("2 !", 2);
            Validate("2! + 3", 5);
            Validate("2 + 3!", 8);
            
            Validate("(2 + 3)! + 2", 122);
            Validate("2! + (2! + (2! + 2)!)", 28);
            Validate("2!*2!*2!", 8);
            Validate("(2 + 2)!*(2 + 2)!", 576);
        }

        [TestMethod]
        public void UtilizeNumberConstants()
        {
            ValidateApproximately("π", 3.1415);
            ValidateApproximately("e", 2.7182);
        }

        private static void Validate(string expressionAsString, double expectedValue)
        {
            var expression = new ExpressionBuilder().WithExpression(expressionAsString).Build();
            if (!expression.IsValid) Assert.Fail(expression.ErrorMessage);

            Assert.AreEqual(expectedValue, expression.GetResult());
        }

        private static void ValidateApproximately(string expressionAsString, double expectedValue)
        {
            var expression = new ExpressionBuilder().WithExpression(expressionAsString).Build();
            if (!expression.IsValid) Assert.Fail(expression.ErrorMessage);

            Assert.AreEqual(expectedValue, expression.GetResult(), 0.001);
        }
    }
}