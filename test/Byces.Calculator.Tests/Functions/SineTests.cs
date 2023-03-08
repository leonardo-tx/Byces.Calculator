using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class SineTests
    {
        [TestMethod]
        public void SineTest()
        {
            Evaluator.ValidateNumber("sin(rad 90)", 1);
            Evaluator.ValidateNumberApproximately("sin(rad 60)", Math.Sqrt(3) / 2, 1E-15);
            Evaluator.ValidateNumberApproximately("sin(rad 120)", Math.Sqrt(3) / 2, 1E-15);
            Evaluator.ValidateNumber("sin(rad 180)", 0);
        }

        [TestMethod]
        public void SineHyperbolicTest()
        {
            Evaluator.ValidateNumberApproximately("sinh10", 11013.2328);
        }
    }
}