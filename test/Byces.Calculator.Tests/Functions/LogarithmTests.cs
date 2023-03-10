using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class LogarithmTests
    {
        [TestMethod]
        public void Logarithm10Test()
        {
            Evaluator.ValidateNumber("log10", Math.Log10(10));
            Evaluator.ValidateNumber("log35", Math.Log10(35));
        }

        [TestMethod]
        public void LogarithmTest()
        {
            Evaluator.ValidateNumber("log(4;2)", 2);
            Evaluator.ValidateNumber("log(91;5)", Math.Log(91, 5));
        }

        [TestMethod]
        public void LogarithmExceptionsTest()
        {
            Evaluator.ValidateException("log(4;-2)", ResultError.Arithmetic);
            Evaluator.ValidateException("log(4;0)", ResultError.Arithmetic);
            Evaluator.ValidateException("log(4;1)", ResultError.Arithmetic);
            Evaluator.ValidateException("log(-4;-2)", ResultError.Arithmetic);
            Evaluator.ValidateException("log(-4;2)", ResultError.Arithmetic);
            Evaluator.ValidateException("log(0;2)", ResultError.Arithmetic);
        }
    }
}