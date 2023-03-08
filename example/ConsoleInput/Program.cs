using Byces.Calculator;
using Byces.Calculator.Interfaces;
using System.Diagnostics;
using System.Text;

namespace ConsoleInput;

public class Program
{
    private readonly static ICalculator _calculator = new CalculatorBuilder().Build();

    public static void Main()
    {
        while (true)
        {
            Console.Write("Enter a math expression: ");
            string input = Console.ReadLine() ?? string.Empty;
            
            var result = _calculator.GetDoubleResult(input);

            if (!result.IsValid)
            {
                Console.WriteLine($"\n{result.ErrorMessage} ({result.ErrorType})\n");
                continue;
            }
            Console.WriteLine($"\nResult: {result.Result}\n");
        }
    }
}