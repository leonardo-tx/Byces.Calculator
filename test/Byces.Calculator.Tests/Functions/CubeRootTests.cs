using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class CubeRootTests
    {
        [TestMethod]
        public void CubeRootTest()
        {
            Evaluator.Validate("cbrt512", Math.Cbrt(512));
            Evaluator.Validate("cbrt-64", Math.Cbrt(-64));
        }
    }
}