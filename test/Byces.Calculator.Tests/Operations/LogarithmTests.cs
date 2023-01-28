using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class LogarithmTests
    {
        [TestMethod]
        public void LogarithmTest()
        {
            Evaluator.Validate("2 log 4", 2);
            Evaluator.Validate("5 log 91", Math.Log(91, 5));
        }

        [TestMethod]
        public void LogarithmExceptionsTest()
        {
            Evaluator.ValidateException("-2 log 4", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("0 log 4", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("1 log 4", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("-2 log -4", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("2 log -4", ResultErrorType.Arithmetic);
            Evaluator.ValidateException("2 log 0", ResultErrorType.Arithmetic);
        }
    }
}