using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class CosineTests
    {
        [TestMethod]
        public void CosineTest()
        {
            Evaluator.ValidateNumber("cos(rad 90)", 0);
            Evaluator.ValidateNumber("cos(rad 60)", 0.5);
            Evaluator.ValidateNumber("cos(rad 120)", -0.5);
            Evaluator.ValidateNumber("cos(rad 180)", -1);
        }

        [TestMethod]
        public void CosineHyperbolicTest()
        {
            Evaluator.ValidateNumberApproximately("cosh10", 11013.2329);
        }
    }
}