using System;
using Byces.Calculator.Enums;
using Byces.Calculator.Interfaces;
using Byces.Calculator.Tests.Settings.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byces.Calculator.Tests
{
    internal static class Evaluator
    {
        static Evaluator()
        {
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(new TestDependency("Test"));
            
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            DefaultCalculator = new CalculatorBuilder()
                .WithAssemblies(typeof(Evaluator).Assembly)
                .WithServiceProvider(serviceProvider)
                .Build();
            CacheCalculator = new CalculatorBuilder()
                .WithAssemblies(typeof(Evaluator).Assembly)
                .WithOptions(CalculatorOptions.CacheExpressions)
                .WithServiceProvider(serviceProvider)
                .Build();
        }

        private static readonly ICalculator DefaultCalculator;
        private static readonly ICalculator CacheCalculator;

        internal static void ValidateNumber(string expressionAsString, double expectedValue)
        {
            MathResult<double> result = DefaultCalculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result);
            Assert.AreEqual(CacheCalculator.GetDoubleResult(expressionAsString), CacheCalculator.GetDoubleResult(expressionAsString));
        }
        
        internal static void ValidateCacheNumber(string expressionAsString, double expectedValue, double delta)
        {
            MathResult<double> result = CacheCalculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);
        }

        internal static void ValidateNumberApproximately(string expressionAsString, double expectedValue, double delta = 1E-4, bool skipCacheEvaluation = false)
        {
            MathResult<double> result = DefaultCalculator.GetDoubleResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);

            Assert.AreEqual(expectedValue, result.Result, delta);

            if (skipCacheEvaluation) return;
            MathResult<double> firstResult = CacheCalculator.GetDoubleResult(expressionAsString);
            MathResult<double> cacheResult = CacheCalculator.GetDoubleResult(expressionAsString);
            
            if (!firstResult.IsValid) Assert.Fail(firstResult.ErrorMessage);
            if (!cacheResult.IsValid) Assert.Fail(cacheResult.ErrorMessage);
            
            Assert.AreEqual(firstResult.Result, cacheResult.Result);
        }

        internal static void ValidateBoolean(string expressionAsString, bool expectedValue)
        {
            MathResult<bool> result = DefaultCalculator.GetBooleanResult(expressionAsString);
            if (!result.IsValid) Assert.Fail(result.ErrorMessage);
            
            Assert.AreEqual(expectedValue, result.Result);

            MathResult<bool> firstResult = CacheCalculator.GetBooleanResult(expressionAsString);
            MathResult<bool> cacheResult = CacheCalculator.GetBooleanResult(expressionAsString);
            
            if (!firstResult.IsValid) Assert.Fail(firstResult.ErrorMessage);
            if (!cacheResult.IsValid) Assert.Fail(cacheResult.ErrorMessage);

            Assert.AreEqual(firstResult.Result, cacheResult.Result);
        }

        internal static void ValidateException(string expressionAsString, ResultError expectedValue)
        {
            MathResult<double> result = DefaultCalculator.GetDoubleResult(expressionAsString);
            Assert.AreEqual(expectedValue, result.Error);
        }
    }
}