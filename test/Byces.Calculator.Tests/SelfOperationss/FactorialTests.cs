using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.SelfOperationss
{
    [TestClass]
    public class FactorialTests
    {
        [TestMethod]
        public void FactorialTest()
        {
            Evaluator.Validate("fact 4", 24);
            Evaluator.Validate("FACT(4 + 5)", 362_880);
            Evaluator.Validate("factfact3", 720);
            Evaluator.Validate("fact2 + (fact2 + fact(fact2 + 2))", 28);
        }
    }
}