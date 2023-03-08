using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Variables
{
    [TestClass]
    public class FalseTests
    {
        [TestMethod]
        public void FalseTest()
        {
            Evaluator.ValidateBoolean("False", false);
            Evaluator.ValidateBoolean("FALSE == (3 == 4)", true);
        }
    }
}