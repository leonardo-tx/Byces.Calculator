using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class SpacingTests
    {
        [TestMethod]
        public void FunctionSpacingTest()
        {
            Evaluator.Validate("c os(ra d9 0)", 0);
            Evaluator.Validate("fa c t (    5 )", 120);
            Evaluator.Validate(" a d d     ( 9 ; 5 ; 3 ) * m i  n ( 5 ; 2 )", 34);
        }

        [TestMethod]
        public void OperationSpacingTest()
        {
            Evaluator.Validate("5 a d d 9", 14);
            Evaluator.Validate("9 D i V 3 m     u                                     L 2", 6);
        }

        [TestMethod]
        public void SpecialNumberSpacingTest()
        {
            Evaluator.Validate("     P         I   ", Math.PI);
            Evaluator.Validate("2 * E u l    ER", Math.E * 2);
        }

        [TestMethod]
        public void NumberSpacingTest()
        {
            Evaluator.Validate("-  2               0 9", -209);
            Evaluator.Validate("+  2  E  +  1   ", 20);
            Evaluator.Validate("- 3 2 * -      1 ", 32);
        }
    }
}