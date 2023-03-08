using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Variables
{
    [TestClass]
    public class TrueTests
    {
        [TestMethod]
        public void TrueTest()
        {
            Evaluator.ValidateBoolean("truE", true);
            Evaluator.ValidateBoolean("true != (2 == 4)", true);
        }
    }
}