using Byces.Calculator;
using Byces.Calculator.Interfaces;

namespace ConsoleInput;

public sealed class Program
{
    private static readonly ICalculator Calculator = new CalculatorBuilder().Build();

    public static void Main()
    {
        while (true)
        {
            Console.Write("Enter a math expression: ");
            string input = Console.ReadLine() ?? string.Empty;

            var result1 = Calculator.GetDoubleResult(input);
            var result2 = Calculator.GetBooleanResult(input);

            if (!result1.IsValid)
            {
                Console.WriteLine($"\n{result1.ErrorMessage} ({result1.Error})\n");
                continue;
            }
            Console.WriteLine($"\nResult (double): {result1.Result}");
            Console.WriteLine($"Result (bool): {result2.Result}\n");
        }
    }
}