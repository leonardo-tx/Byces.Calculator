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
            Evaluator.ValidateApproximately("(2 + 6 - (13 * 24 + 5 / (123 - 364 + 23))) - (2 + 6 - (13 * 24 + 5 / (123 - 364 + 23))) + (2 + 6 - (13 * 24 + 5 / (123 - 364 + 23))) * 345 * ((897 - 323)/ 23)", -2_617_242.5229);
        }
    }
}