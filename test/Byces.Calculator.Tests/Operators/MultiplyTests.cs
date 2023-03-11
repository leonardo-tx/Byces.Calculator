using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Operators
{
    [TestClass]
    public class MultiplyTests
    {
        [TestMethod]
        public void MultiplyTest1()
        {
            Evaluator.ValidateNumber("2*3*9", 54);
            Evaluator.ValidateNumber("5 * 7* 10", 350);
        }

        [TestMethod]
        public void MultiplyTest2()
        {
            Evaluator.ValidateNumber("2mul3MUl9", 54);
            Evaluator.ValidateNumber("5 MUL 7mUL 10", 350);
        }

        [TestMethod]
        public void MultiplyWithSignsTest1()
        {
            Evaluator.ValidateNumber("-2*4", -8);
            Evaluator.ValidateNumber("+4 * -5 *-5", 100);
        }

        [TestMethod]
        public void MultiplyWithSignsTest2()
        {
            Evaluator.ValidateNumber("-2mUl4", -8);
            Evaluator.ValidateNumber("+4 MuL -5 mUL-5", 100);
        }

        [TestMethod]
        public void ImplicitMultiplyTest()
        {
            Evaluator.ValidateNumber("2(5 + 3)", 16);
            Evaluator.ValidateNumber("PI(6 + 2(3-5))", Math.PI * 2);
        }
    }
}