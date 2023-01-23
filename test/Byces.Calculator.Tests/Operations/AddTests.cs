using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class AddTests
    {
        [TestMethod]
        public void AddTest1()
        {
            Evaluator.Validate("2+5+3", 10);
            Evaluator.Validate("102 + 76 + 42", 220);
        }

        [TestMethod]
        public void AddTest2()
        {
            Evaluator.Validate("2Add5add3", 10);
            Evaluator.Validate("102 ADD 76 + 42", 220);
        }

        [TestMethod]
        public void AddWithSignsTest1()
        {
            Evaluator.Validate("-2++30++53", 81);
            Evaluator.Validate("11 + -10 + -2", -1);
        }

        [TestMethod]
        public void AddWithSignsTest2()
        {
            Evaluator.Validate("-2add+30add+53", 81);
            Evaluator.Validate("11 + -10 AdD -2", -1);
        }
    }
}