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
            Evaluator.Validate("sin(rad 90)", 1);
            Evaluator.ValidateApproximately("sin(rad 60)", Math.Sqrt(3) / 2, 1E-15);
            Evaluator.ValidateApproximately("sin(rad 120)", Math.Sqrt(3) / 2, 1E-15);
            Evaluator.Validate("sin(rad 180)", 0);
        }

        [TestMethod]
        public void SineHyperbolicTest()
        {
            Evaluator.ValidateApproximately("sinh10", 11013.2328);
        }
    }
}