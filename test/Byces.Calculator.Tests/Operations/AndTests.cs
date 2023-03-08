using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class AndTests
    {
        [TestMethod]
        public void AndTest()
        {
            Evaluator.ValidateBoolean("2 == 3 && 5 == 4", false);
            Evaluator.ValidateBoolean("true && 9 == 28", false);
            Evaluator.ValidateBoolean("false && 2 == 2", false);
            Evaluator.ValidateBoolean("true&&9!=2", true);
        }

        [TestMethod]
        public void AndExceptionsTest()
        {
            Evaluator.ValidateException("2 && true", ResultErrorType.InvalidArgument);
            Evaluator.ValidateException("2 && 5", ResultErrorType.InvalidArgument);
            Evaluator.ValidateException("false && 88", ResultErrorType.InvalidArgument);
        }
    }
}