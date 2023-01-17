using Byces.Calculator;

namespace ConsoleInput;

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("Enter a math expression: ");
            var expression = ExpressionBuilder.GetMathExpression(Console.ReadLine() ?? string.Empty);
            if (!expression.IsValid)
            {
                Console.WriteLine($"\n{expression.ErrorMessage}\n");
                continue;
            }
            Console.WriteLine($"\nResult: {expression.GetResult()}\n");
        }
    }
}