using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Operations
{
    [TestClass]
    public class ModulusTests
    {
        [TestMethod]
        public void ModulusTest1()
        {
            Evaluator.Validate("4%2", 0);
            Evaluator.Validate("5 % 2 % 5", 1);
        }

        [TestMethod]
        public void ModulusTest2()
        {
            Evaluator.Validate("4MOD2", 0);
            Evaluator.Validate("5 mod 2 mOd 5", 1);
        }

        [TestMethod]
        public void ModulusTestWithSigns1()
        {
            Evaluator.Validate("3%-2", -1);
            Evaluator.Validate("-5 % +0", -5);
        }

        [TestMethod]
        public void ModulusTestWithSigns2()
        {
            Evaluator.Validate("3moD-2", -1);
            Evaluator.Validate("-5 MOd +0", -5);
        }
    }
}