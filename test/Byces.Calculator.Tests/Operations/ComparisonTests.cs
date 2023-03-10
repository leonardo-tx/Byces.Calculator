using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class ComparisonTests
    {
        [TestMethod]
        public void EqualTest()
        {
            Evaluator.ValidateBoolean("2 == 32", false);
            Evaluator.ValidateBoolean("5 == 5", true);
            Evaluator.ValidateBoolean("true == true", true);
            Evaluator.ValidateBoolean("true == false", false);
            Evaluator.ValidateBoolean("5 == true", false);
        }

        [TestMethod]
        public void NotEqualTest()
        {
            Evaluator.ValidateBoolean("2 != 32", true);
            Evaluator.ValidateBoolean("5 != 5", false);
            Evaluator.ValidateBoolean("true != true", false);
            Evaluator.ValidateBoolean("true != false", true);
            Evaluator.ValidateBoolean("5 != true", true);
        }

        [TestMethod]
        public void GreaterTest()
        {
            Evaluator.ValidateBoolean("5 > 4", true);
            Evaluator.ValidateBoolean("3 > 9", false);
            Evaluator.ValidateBoolean("6 > 6", false);
        }

        [TestMethod]
        public void GreaterEqualTest()
        {
            Evaluator.ValidateBoolean("5 >= 4", true);
            Evaluator.ValidateBoolean("3 >= 9", false);
            Evaluator.ValidateBoolean("6 >= 6", true);
        }

        [TestMethod]
        public void LessTest()
        {
            Evaluator.ValidateBoolean("5 < 4", false);
            Evaluator.ValidateBoolean("3 < 9", true);
            Evaluator.ValidateBoolean("6 < 6", false);
        }

        [TestMethod]
        public void LessEqualTest()
        {
            Evaluator.ValidateBoolean("5 <= 4", false);
            Evaluator.ValidateBoolean("3 <= 9", true);
            Evaluator.ValidateBoolean("6 <= 6", true);
        }
    }
}