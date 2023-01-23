using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.SelfOperationss
{
    [TestClass]
    public class CosineTests
    {
        [TestMethod]
        public void CosineTest()
        {
            Evaluator.Validate("cos(rad 90)", 0);
            Evaluator.Validate("cos(rad 60)", 0.5);
            Evaluator.Validate("cos(rad 120)", -0.5);
            Evaluator.Validate("cos(rad 180)", -1);
        }

        [TestMethod]
        public void CosineHyperbolicTest()
        {
            Evaluator.ValidateApproximately("cosh10", 11013.2329);
        }
    }
}