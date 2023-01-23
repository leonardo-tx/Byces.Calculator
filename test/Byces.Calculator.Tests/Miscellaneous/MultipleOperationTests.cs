using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class MultipleOperationTests
    {
        [TestMethod]
        public void MultipleOperationTest1()
        {
            Evaluator.Validate("2 + 7 * 5", 37);
            Evaluator.Validate("2 ^ 2 + (4 + 5 * (2 √ 9))", 23);
            Evaluator.ValidateApproximately("2 + 4 - 8 * 7 / 9 ^ 2 √ 1000", -45.4222);
        }
    }
}