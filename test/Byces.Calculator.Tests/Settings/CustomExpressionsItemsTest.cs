using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Settings;

[TestClass]
public class CustomExpressionsItemsTest
{
    [TestMethod]
    public void CustomVariablesTest()
    {
        Evaluator.ValidateNumber("two + two", 4);
        Evaluator.ValidateNumberApproximately("RANDOMNumBEr", 4.5, 4.5, true);
        Evaluator.ValidateCacheNumber("-RANDOMNumBEr", -4.5, 4.5);
        Evaluator.ValidateCacheNumber("-RANDOMNumBEr", -4.5, 4.5);
        Evaluator.ValidateNumberApproximately("?", 1073741824, 1073741824, true);
    }
    
    [TestMethod]
    public void CustomDependencyVariablesTest()
    {
        Evaluator.ValidateBoolean("DEPENDENCY", false);
    }
}