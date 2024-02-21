using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests.Settings;

[TestClass]
public class CacheTest
{
    [TestMethod]
    public void GetCachedCountTest()
    {
        ICalculator calculator = new CalculatorBuilder().WithOptions(CalculatorOptions.CacheExpressions).Build();
        
        Assert.AreEqual(0, calculator.CachedCount);
        calculator.GetDoubleResult("2 + 2");
        calculator.GetDoubleResult("2+2");
        Assert.AreEqual(1, calculator.CachedCount);
        
        calculator.GetDoubleResult("3 + 5 * 4");
        Assert.AreEqual(2, calculator.CachedCount);

        calculator.GetDoubleResult("9 +");
        Assert.AreEqual(2, calculator.CachedCount);
    }

    [TestMethod]
    public void ClearCacheTest()
    {
        ICalculator calculator = new CalculatorBuilder().WithOptions(CalculatorOptions.CacheExpressions).Build();
        
        calculator.GetDoubleResult("2 + 2");
        calculator.GetDoubleResult("3 + 5 * 4");
        
        calculator.FreeExpressionsCache();
        Assert.AreEqual(0, calculator.CachedCount);
        
        calculator.GetDoubleResult("2 + 2");
        Assert.AreEqual(1, calculator.CachedCount);
    }
}