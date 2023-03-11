using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operators
{
    [TestClass]
    public class PowerTests
    {
        [TestMethod]
        public void PowerTest1()
        {
            Evaluator.ValidateNumber("2^32", 4_294_967_296);
            Evaluator.ValidateNumber("3 ^ 0 ^ 4", 1);
        }

        [TestMethod]
        public void PowerTest2()
        {
            Evaluator.ValidateNumber("2pow32", 4_294_967_296);
            Evaluator.ValidateNumber("3 POW 0 Pow 4", 1);
        }

        [TestMethod]
        public void PowerWithSigns1()
        {
            Evaluator.ValidateNumber("-3 ^ 5", -243);
            Evaluator.ValidateNumberApproximately("+3 ^ -2", 0.1111);
        }

        [TestMethod]
        public void PowerWithSigns2()
        {
            Evaluator.ValidateNumber("-3 pOw 5", -243);
            Evaluator.ValidateNumberApproximately("+3 PoW -2", 0.1111);
        }
    }
}