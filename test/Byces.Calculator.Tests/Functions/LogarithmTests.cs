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
        public void LogarithmNegativeTest()
        {
            Evaluator.ValidateNumber("log(4;-2)", Math.Log(4, -2));;
            Evaluator.ValidateNumber("log(4;0)", Math.Log(4, 0));
            Evaluator.ValidateNumber("log(4;1)", Math.Log(4, 1));
            Evaluator.ValidateNumber("log(-4;-2)", Math.Log(-4, -2));
            Evaluator.ValidateNumber("log(-4;2)", Math.Log(-4, 2));
            Evaluator.ValidateNumber("log(0;2)", Math.Log(0, 2));
        }
    }
}