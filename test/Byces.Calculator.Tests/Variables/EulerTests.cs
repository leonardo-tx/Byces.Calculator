using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Variables
{
    [TestClass]
    public class EulerTests
    {
        [TestMethod]
        public void EulerTest()
        {
            Evaluator.ValidateNumber("Euler", Math.E);
            Evaluator.ValidateNumber("-Euler", -Math.E);
            Evaluator.ValidateNumber("2 * Euler", Math.E * 2);
        }
    }
}