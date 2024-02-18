using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class FactorialTests
    {
        [TestMethod]
        public void FactorialTest()
        {
            Evaluator.ValidateNumber("(fact(3)) + 3", 9);
            Evaluator.ValidateNumber("fact 4", 24);
            Evaluator.ValidateNumber("FACT(4 + 5)", 362_880);
            Evaluator.ValidateNumber("factfact3", 720);
            Evaluator.ValidateNumber("fact2 + (fact2 + fact(fact2 + 2))", 28);
        }
    }
}