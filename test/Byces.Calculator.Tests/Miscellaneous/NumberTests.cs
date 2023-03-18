using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void EmptyNumberTest()
        {
            Evaluator.ValidateNumber("", 0);
            Evaluator.ValidateNumber("            ", 0);
        }

        [TestMethod]
        public void SingleNumberTest()
        {
            Evaluator.ValidateNumber("4", 4);
            Evaluator.ValidateNumber("  5 ", 5);
            Evaluator.ValidateNumber("( (8))", 8);
            Evaluator.ValidateNumber("-9", -9);
        }

        [TestMethod]
        public void ScientificNotationTest()
        {
            Evaluator.ValidateNumber("2E+1", 2E+1);
            Evaluator.ValidateNumber("-2E-2", -2E-2);
            Evaluator.ValidateNumber("3E+3 + 3", 3E+3 + 3);
        }

        [TestMethod]
        public void NumberDecimalSeparatorTest()
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            for (int i = 0; i < cultures.Length; i++)
            {
                string decimalSeparator = cultures[i].NumberFormat.NumberDecimalSeparator;

                Thread.CurrentThread.CurrentCulture = cultures[i];

                Evaluator.ValidateNumber($"4{decimalSeparator}5 + 5{decimalSeparator}5", 10);
                Evaluator.ValidateNumberApproximately($"fact 2{decimalSeparator}6", 3.7170);
                Evaluator.ValidateNumber($"5{decimalSeparator} + 3", 8);
                Evaluator.ValidateNumber($"{decimalSeparator}9 + 10", 10.9);
            }
        }
    }
}