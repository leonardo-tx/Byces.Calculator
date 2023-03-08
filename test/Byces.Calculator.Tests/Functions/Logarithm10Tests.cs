using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class Logarithm10Tests
    {
        [TestMethod]
        public void Logarithm10Test()
        {
            Evaluator.ValidateNumber("log10", Math.Log10(10));
            Evaluator.ValidateNumber("log35", Math.Log10(35));
        }
    }
}