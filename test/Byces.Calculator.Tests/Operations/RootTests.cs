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
            Evaluator.Validate("2√9", 3);
            Evaluator.Validate("2 √ 9 √ 64", 4);
        }

        [TestMethod]
        public void RootTest2()
        {
            Evaluator.Validate("2rt9", 3);
            Evaluator.Validate("2 rt 9 rt 64", 4);
        }

        [TestMethod]
        public void RootWithSignsTest1()
        {
            Evaluator.Validate("3√-64", -4);
            Evaluator.Validate("+5 √ -1024", -4);
        }

        [TestMethod]
        public void RootWithSignsTest2()
        {
            Evaluator.Validate("3rt-64", -4);
            Evaluator.Validate("+5 rt -1024", -4);
        }

        [TestMethod]
        public void RootExceptionsTest()
        {
            Evaluator.ValidateException("2rt-64", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("-3rt64", ResultErrorType.Arithmetic);
        }
    }
}