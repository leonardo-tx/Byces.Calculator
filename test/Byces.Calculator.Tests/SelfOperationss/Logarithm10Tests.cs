using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.SelfOperationss
{
    [TestClass]
    public class Logarithm10Tests
    {
        [TestMethod]
        public void Logarithm10Test()
        {
            Evaluator.Validate("log10", Math.Log10(10));
            Evaluator.Validate("log35", Math.Log10(35));
        }
    }
}