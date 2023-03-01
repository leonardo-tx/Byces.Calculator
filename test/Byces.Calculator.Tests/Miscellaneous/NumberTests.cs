using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void EmptyNumberTest()
        {
            Evaluator.Validate("", 0);
            Evaluator.Validate("            ", 0);
        }

        [TestMethod]
        public void SingleNumberTest()
        {
            Evaluator.Validate("4", 4);
            Evaluator.Validate("  5 ", 5);
            Evaluator.Validate("( (8))", 8);
            Evaluator.Validate("-9", -9);
        }

        [TestMethod]
        public void ScientificNotationTest()
        {
            Evaluator.Validate("2E+1", 2E+1);
            Evaluator.Validate("-2E-2", -2E-2);
            Evaluator.Validate("3E+3 + 3", 3E+3 + 3);
        }

        [TestMethod]
        public void NumberDecimalSeparatorTest()
        {
            switch (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0])
            {
                case ',':
                    Evaluator.Validate("4,5 + 5,5", 10);
                    Evaluator.ValidateApproximately("fact 2,6", 3.7170);
                    Evaluator.Validate("5, + 3", 8);
                    Evaluator.Validate(",9 + 10", 10.9);
                    break;
                case '.':
                    Evaluator.Validate("4.5 + 5.5", 10);
                    Evaluator.ValidateApproximately("fact 2.6", 3.7170);
                    Evaluator.Validate("5. + 3", 8);
                    Evaluator.Validate(".9 + 10", 10.9);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}