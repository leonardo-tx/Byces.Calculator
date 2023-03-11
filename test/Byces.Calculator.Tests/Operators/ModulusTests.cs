using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operators
{
    [TestClass]
    public class ModulusTests
    {
        [TestMethod]
        public void ModulusTest1()
        {
            Evaluator.ValidateNumber("4%2", 0);
            Evaluator.ValidateNumber("5 % 2 % 5", 1);
        }

        [TestMethod]
        public void ModulusTest2()
        {
            Evaluator.ValidateNumber("4MOD2", 0);
            Evaluator.ValidateNumber("5 mod 2 mOd 5", 1);
        }

        [TestMethod]
        public void ModulusTestWithSigns1()
        {
            Evaluator.ValidateNumber("3%-2", -1);
            Evaluator.ValidateNumber("-5 % +0", -5);
        }

        [TestMethod]
        public void ModulusTestWithSigns2()
        {
            Evaluator.ValidateNumber("3moD-2", -1);
            Evaluator.ValidateNumber("-5 MOd +0", -5);
        }
    }
}