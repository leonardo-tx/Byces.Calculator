using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class LogicalTests
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
            Evaluator.ValidateException("2 && true", ResultError.InvalidArgument);
            Evaluator.ValidateException("2 && 5", ResultError.InvalidArgument);
            Evaluator.ValidateException("false && 88", ResultError.InvalidArgument);
        }

        [TestMethod]
        public void OrTest()
        {
            Evaluator.ValidateBoolean("2 == 3 || 5 == 4", false);
            Evaluator.ValidateBoolean("true || 9 == 28", true);
            Evaluator.ValidateBoolean("false || 2 == 2", true);
            Evaluator.ValidateBoolean("true||9!=2", true);
        }

        [TestMethod]
        public void OrExceptionsTest()
        {
            Evaluator.ValidateException("2 || true", ResultError.InvalidArgument);
            Evaluator.ValidateException("2 || 5", ResultError.InvalidArgument);
            Evaluator.ValidateException("false || 88", ResultError.InvalidArgument);
        }
    }
}