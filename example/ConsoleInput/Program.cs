using Byces.Calculator;

namespace ConsoleInput;

public class Program
{
    private readonly static ICalculator _calculator = new CalculatorBuilder().Build();

    public static void Main()
    {
        while (true)
        {
            Console.Write("Enter a math expression: ");
            var expression = _calculator.GetDoubleResult(Console.ReadLine() ?? string.Empty);
            if (!expression.IsValid)
            {
                Console.WriteLine($"\n{expression.ErrorMessage} ({expression.ErrorType})\n");
                continue;
            }
            Console.WriteLine($"\nResult: {expression.Result}\n");
        }
    }
}