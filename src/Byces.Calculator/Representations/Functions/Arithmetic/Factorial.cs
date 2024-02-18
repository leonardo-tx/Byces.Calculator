using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Arithmetic
{
    internal sealed class Factorial : FunctionRepresentation
    {
        public override string StringRepresentation => "FACT";
        public override int ParametersMax => 1;
        
        public override bool Pure => true;

        public override Variable Operate(ReadOnlySpan<Variable> variables)
        {
            double number = variables[0].Double;

            double difference = number - Math.Floor(number);
            if (difference == 0) return GetFactorial((int)number);
            
            return number < 0 ? -GetGamma(-number + 1) : GetGamma(number + 1);
        }

        private static double GetFactorial(int number)
        {
            if (number < 0) return -GetFactorial(-number);
            if (number > 170) return double.PositiveInfinity;

            double result = 1;
            for (int i = 2; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }

        // Credits for the Gamma function implementation below: https://rosettacode.org/wiki/Gamma_function

        private static readonly int g = 7;
        private static readonly double[] p = 
        { 
            0.99999999999980993, 676.5203681218851, -1259.1392167224028,
            771.32342877765313, -176.61502916214059, 12.507343278686905,
            -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7 
        };

        private static double GetGamma(double number)
        {
            if (number < 0.5)
            {
                return Math.PI / (Math.Sin(Math.PI * number) * GetGamma(1 - number));
            }
            number -= 1;
            double x = p[0];

            for (var i = 1; i < g + 2; i++)
            {
                x += p[i] / (number + i);
            }

            double t = number + g + 0.5;
            return Math.Sqrt(2 * Math.PI) * Math.Pow(t, number + 0.5) * Math.Exp(-t) * x;
        }
    }
}