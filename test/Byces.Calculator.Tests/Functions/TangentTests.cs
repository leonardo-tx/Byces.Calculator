using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Byces.Calculator.Tests.Functions
{
    [TestClass]
    public class TangentTests
    {
        [TestMethod]
        public void TangentTest()
        {
            Evaluator.ValidateNumberApproximately("tan(rad 60)", Math.Sqrt(3), 1E-15);
            Evaluator.ValidateNumberApproximately("tan(rad 120)", Math.Sqrt(3) * -1, 1E-15);
            Evaluator.ValidateNumber("tan(rad 45)", 1);
        }

        [TestMethod]
        public void TangentExceptionsTest()
        {
            Evaluator.ValidateException("tan(rad 90)", ResultError.Arithmetic);
            Evaluator.ValidateException("tan(rad 450)", ResultError.Arithmetic);
        }
    }
}