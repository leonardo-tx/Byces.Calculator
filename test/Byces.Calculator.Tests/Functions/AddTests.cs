using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class AddTests
    {
        [TestMethod]
        public void AddTest()
        {
            Evaluator.Validate("add(2;9;5)", 16);
            Evaluator.Validate("add(5; 3; 2) - add(1; 2)", 7);
        }

        [TestMethod]
        public void AddTestSingle()
        {
            Evaluator.Validate("add(2)", 2);
        }
    }
}