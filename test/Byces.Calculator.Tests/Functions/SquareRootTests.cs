using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class SquareRootTests
    {
        [TestMethod]
        public void SquareRootTest1()
        {
            Evaluator.Validate("√9", 3);
        }

        [TestMethod]
        public void SquareRootTest2()
        {
            Evaluator.Validate("sqrt(9)", 3);
        }

        [TestMethod]
        public void SquareRootExceptionsTest()
        {
            Evaluator.ValidateException("√-9", ResultErrorType.Arithmetic);
        }
    }
}