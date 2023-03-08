using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class RootTests
    {
        [TestMethod]
        public void RootTest1()
        {
            Evaluator.ValidateNumber("2√9", 3);
            Evaluator.ValidateNumber("2 √ 9 √ 64", 4);
        }

        [TestMethod]
        public void RootTest2()
        {
            Evaluator.ValidateNumber("2rt9", 3);
            Evaluator.ValidateNumber("2 rt 9 rt 64", 4);
        }

        [TestMethod]
        public void RootWithSignsTest1()
        {
            Evaluator.ValidateNumber("3√-64", -4);
            Evaluator.ValidateNumber("+5 √ -1024", -4);
        }

        [TestMethod]
        public void RootWithSignsTest2()
        {
            Evaluator.ValidateNumber("3rt-64", -4);
            Evaluator.ValidateNumber("+5 rt -1024", -4);
        }

        [TestMethod]
        public void RootExceptionsTest()
        {
            Evaluator.ValidateException("2rt-64", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("-3rt64", ResultErrorType.Arithmetic);
        }
    }
}