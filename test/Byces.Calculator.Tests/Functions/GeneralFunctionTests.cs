using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class GeneralFunctionTests
    {
        [TestMethod]
        public void DecreasingNumberListBeforeFunctionTest()
        {
            Evaluator.ValidateApproximately("(1 + cos90) + 1 + 1 + cos90", 2.1038);
            Evaluator.Validate("(aDD(2;5;3) * 2) + 4 - min(9;8)", 16);
        }
    }
}