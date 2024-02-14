using Byces.Calculator.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Miscellaneous
{
    [TestClass]
    public class GeneralExceptionTests
    {
        [TestMethod]
        public void PriorityExceptions()
        {
            Evaluator.ValidateException("(2 + 2) + (3 + 4", ResultError.MissingParentheses);
            Evaluator.ValidateException("(2 + 3 * (4 - add(2; 5; 1; 7; 8))", ResultError.MissingParentheses);
            
            Evaluator.ValidateException("() + 2", ResultError.MissingParentheses);
            Evaluator.ValidateException("2 / 4 ^ (5(4 + 5)))", ResultError.MisplacedParentheses);
        }
    }
}