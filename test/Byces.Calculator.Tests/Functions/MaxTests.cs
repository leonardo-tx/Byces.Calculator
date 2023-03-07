using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class MaxTests
    {
        [TestMethod]
        public void MaxTest()
        {
            Evaluator.Validate("max(-2;-5;-11)", -2);
            Evaluator.Validate("max(2;8;5;4) * max(2;5)", 40);
        }

        [TestMethod]
        public void MaxTestSingle()
        {
            Evaluator.Validate("max(2)", 2);
        }
    }
}