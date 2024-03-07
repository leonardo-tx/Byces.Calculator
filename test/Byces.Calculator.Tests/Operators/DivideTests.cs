using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operators
{
    [TestClass]
    public class DivideTests
    {
        [TestMethod]
        public void DivideTest1()
        {
            Evaluator.ValidateNumber("12 / 4", 3);
            Evaluator.ValidateNumber("42/2/3", 7);
        }

        [TestMethod]
        public void DivideTest2()
        {
            Evaluator.ValidateNumber("12 DIV 4", 3);
            Evaluator.ValidateNumber("42div2dIV3", 7);
        }

        [TestMethod]
        public void DivideWithSignsTest1()
        {
            Evaluator.ValidateNumberApproximately("+10/-3", -3.3333);
            Evaluator.ValidateNumber("-3 / 2", -1.5);
        }

        [TestMethod]
        public void DivideWithSignsTest2()
        {
            Evaluator.ValidateNumberApproximately("+10Div-3", -3.3333);
            Evaluator.ValidateNumber("-3 DiV 2", -1.5);
        }
    }
}