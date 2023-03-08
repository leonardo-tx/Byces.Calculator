using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class NotTests
    {
        [TestMethod]
        public void NotTest()
        {
            Evaluator.ValidateBoolean("!true", false);
            Evaluator.ValidateBoolean("! (2 == 5)", true);
        }

        [TestMethod]
        public void NotExceptionsTest()
        {
            Evaluator.ValidateException("!2", ResultErrorType.InvalidArgument);
        }
    }
}