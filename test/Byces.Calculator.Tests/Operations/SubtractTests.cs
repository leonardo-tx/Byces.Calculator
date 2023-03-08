using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class SubtractTests
    {
        [TestMethod]
        public void SubtractTest1()
        {
            Evaluator.ValidateNumber("7-5-41", -39);
            Evaluator.ValidateNumber("10 - 3", 7);
        }

        [TestMethod]
        public void SubtractTest2()
        {
            Evaluator.ValidateNumber("7sub5SUB41", -39);
            Evaluator.ValidateNumber("10 sUb 3", 7);
        }

        [TestMethod]
        public void SubtractWithSignsTest1()
        {
            Evaluator.ValidateNumber("+5--2--3", 10);
            Evaluator.ValidateNumber("-38 - 6 - -2", -42);
        }

        [TestMethod]
        public void SubtractWithSignsTest2()
        {
            Evaluator.ValidateNumber("+5sub-2Sub-3", 10);
            Evaluator.ValidateNumber("-38 SUB 6 sUB -2", -42);
        }
    }
}