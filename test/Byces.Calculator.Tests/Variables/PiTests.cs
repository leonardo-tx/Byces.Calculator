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
            Evaluator.ValidateNumber("π", Math.PI);
            Evaluator.ValidateNumber("-π", -Math.PI);
            Evaluator.ValidateNumber("2 * π", Math.PI * 2);
        }

        [TestMethod]
        public void PiTest2()
        {
            Evaluator.ValidateNumber("PI", Math.PI);
            Evaluator.ValidateNumber("-PI", -Math.PI);
            Evaluator.ValidateNumber("2 * PI", Math.PI * 2);
        }
    }
}