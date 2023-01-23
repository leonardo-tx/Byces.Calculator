using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void EmptyNumberTest()
        {
            Evaluator.Validate("", 0);
            Evaluator.Validate("            ", 0);
        }

        [TestMethod]
        public void SingleNumberTest()
        {
            Evaluator.Validate("4", 4);
            Evaluator.Validate("  5 ", 5);
            Evaluator.Validate("( (8))", 8);
            Evaluator.Validate("-9", -9);
        }
    }
}