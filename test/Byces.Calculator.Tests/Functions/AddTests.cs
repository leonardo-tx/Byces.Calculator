using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class AddTests
    {
        [TestMethod]
        public void AddTest()
        {
            Evaluator.ValidateNumber("add(2;9;5)", 16);
            Evaluator.ValidateNumber("add(5; 3; 2) - add(1; 2)", 7);
        }

        [TestMethod]
        public void AddTestSingle()
        {
            Evaluator.ValidateNumber("add(2)", 2);
        }

        [TestMethod]
        public void AddExceptionsTest()
        {
            Evaluator.ValidateException("add(2 == 2)", Enums.ResultErrorType.InvalidArgument);
        }
    }
}