using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Variables
{
    [TestClass]
    public class PiTests
    {
        [TestMethod]
        public void PiTest1()
        {
            Evaluator.Validate("π", Math.PI);
            Evaluator.Validate("-π", -Math.PI);
            Evaluator.Validate("2 * π", Math.PI * 2);
        }

        [TestMethod]
        public void PiTest2()
        {
            Evaluator.Validate("PI", Math.PI);
            Evaluator.Validate("-PI", -Math.PI);
            Evaluator.Validate("2 * PI", Math.PI * 2);
        }
    }
}