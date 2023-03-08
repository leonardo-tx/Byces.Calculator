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
            Evaluator.ValidateNumber("c os(ra d9 0)", 0);
            Evaluator.ValidateNumber("cos h(90)", Math.Cosh(90));
            Evaluator.ValidateNumber("fa c t (    5 )", 120);
            Evaluator.ValidateNumber(" a d d     ( 9 ; 5 ; 3 ) * m i  n ( 5 ; 2 )", 34);
        }

        [TestMethod]
        public void OperationSpacingTest()
        {
            Evaluator.ValidateNumber("5 a d d 9", 14);
            Evaluator.ValidateNumber("9 D i V 3 m     u                                     L 2", 6);
        }

        [TestMethod]
        public void VariableSpacingTest()
        {
            Evaluator.ValidateNumber("     P         I   ", Math.PI);
            Evaluator.ValidateNumber("2 * E u l    ER", Math.E * 2);
        }

        [TestMethod]
        public void NumberSpacingTest()
        {
            Evaluator.ValidateNumber("-  2               0 9", -209);
            Evaluator.ValidateNumber("+  2  E  +  1   ", 20);
            Evaluator.ValidateNumber("- 3 2 * -      1 ", 32);
        }
    }
}