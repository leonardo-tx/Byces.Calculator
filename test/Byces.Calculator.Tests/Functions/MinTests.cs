using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class MinTests
    {
        [TestMethod]
        public void MinTest()
        {
            Evaluator.ValidateNumber("min(2;4;5;-5;9;10)", -5);
            Evaluator.ValidateNumber("min(10;1) + min(2;5;4)", 3);
        }

        [TestMethod]
        public void MinTestSingle()
        {
            Evaluator.ValidateNumber("min(2)", 2);
        }
    }
}