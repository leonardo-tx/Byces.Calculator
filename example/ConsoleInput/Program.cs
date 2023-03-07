using Byces.Calculator;
using Byces.Calculator.Interfaces;
using System.Diagnostics;

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
            
            long initialTime = Stopwatch.GetTimestamp();
            var result = _calculator.GetDoubleResult(input);
            TimeSpan timeSpan = Stopwatch.GetElapsedTime(initialTime);

            if (!result.IsValid)
            {
                Console.WriteLine($"\n{result.ErrorMessage} ({result.ErrorType})\n");
                continue;
            }
            Console.WriteLine($"\nResult in {timeSpan.TotalMilliseconds} ms: {result.Result}\n");
        }
    }
}