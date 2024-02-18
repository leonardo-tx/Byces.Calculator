using System;
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
            Evaluator.ValidateNumber("√9", 3);
        }

        [TestMethod]
        public void SquareRootTest2()
        {
            Evaluator.ValidateNumber("sqrt(9)", 3);
        }

        [TestMethod]
        public void SquareRootExceptionsTest()
        {
            Evaluator.ValidateNumber("√-9", Math.Sqrt(-9));
        }
    }
}