using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.SelfOperationss
{
    [TestClass]
    public class CubeRootTests
    {
        [TestMethod]
        public void CubeRootTest()
        {
            Evaluator.Validate("cbrt512", 8);
            Evaluator.Validate("cbrt-64", -4);
        }
    }
}