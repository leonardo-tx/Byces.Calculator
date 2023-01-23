using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class MultiplyTests
    {
        [TestMethod]
        public void MultiplyTest1()
        {
            Evaluator.Validate("2*3*9", 54);
            Evaluator.Validate("5 * 7* 10", 350);
        }

        [TestMethod]
        public void MultiplyTest2()
        {
            Evaluator.Validate("2mul3MUl9", 54);
            Evaluator.Validate("5 MUL 7mUL 10", 350);
        }

        [TestMethod]
        public void MultiplyWithSigns1()
        {
            Evaluator.Validate("-2*4", -8);
            Evaluator.Validate("+4 * -5 *-5", 100);
        }

        [TestMethod]
        public void MultiplyWithSigns2()
        {
            Evaluator.Validate("-2mUl4", -8);
            Evaluator.Validate("+4 MuL -5 mUL-5", 100);
        }
    }
}