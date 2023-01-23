using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.SpecialNumbers
{
    [TestClass]
    public class EulerTests
    {
        [TestMethod]
        public void EulerTest()
        {
            Evaluator.Validate("Euler", Math.E);
            Evaluator.Validate("-Euler", -Math.E);
            Evaluator.Validate("2 * Euler", Math.E * 2);
        }
    }
}