using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class DivideTests
    {
        [TestMethod]
        public void DivideTest1()
        {
            Evaluator.Validate("12 / 4", 3);
            Evaluator.Validate("42/2/3", 7);
        }

        [TestMethod]
        public void DivideTest2()
        {
            Evaluator.Validate("12 DIV 4", 3);
            Evaluator.Validate("42div2dIV3", 7);
        }

        [TestMethod]
        public void DivideWithSignsTest1()
        {
            Evaluator.ValidateApproximately("+10/-3", -3.3333);
            Evaluator.Validate("-3 / 2", -1.5);
        }

        [TestMethod]
        public void DivideWithSignsTest2()
        {
            Evaluator.ValidateApproximately("+10Div-3", -3.3333);
            Evaluator.Validate("-3 DiV 2", -1.5);
        }

        [TestMethod]
        public void DivideExceptionsTest()
        {
            Evaluator.ValidateException("10 / 0", ResultErrorType.Arithmetic);
        }
    }
}