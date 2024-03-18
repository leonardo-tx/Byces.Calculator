# Byces.Calculator
[![NuGet version](https://img.shields.io/nuget/v/Byces.Calculator)](https://www.nuget.org/packages/Byces.Calculator)
[![.NET](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/29682/badge.svg)](https://scan.coverity.com/projects/leonardo-tx-byces-calculator)

A simple .NET calculator that solves expressions

1. [Usage example](#usage-example)
2. [Building expressions](#building-expressions)
3. [Available operators](#available-operators)
4. [Available functions](#available-functions)
5. [Available variables](#available-variables)
6. [Building your own functions and variables](#building-your-own-functions-and-variables)
7. [Benchmark](#benchmark-100000-iterations)
8. [Future goals](#future-goals)
9. [Feedback and bugs](#feedback-and-bugs)

## Usage example

```csharp
ICalculator calculator = new CalculatorBuilder().Build();

string expression = "2 + 5 * 3";
var mathResult = calculator.GetDoubleResult(expression);

if (mathResult.IsValid) // true
{
    Console.WriteLine(mathResult.Result); // 17
}
else
{
    Console.WriteLine(mathResult.ErrorMessage);
}
```

## Building expressions

Before showing some syntax examples, here are some observations:

* Numbers, variables, operators and functions are not case sensitive
* The calculator is not sensitive to whitespace by default and using it has a low impact on performance

```csharp
// You can use signs to represent negative and positive numbers
string expressionExample1 = "-5 + +5";

// You can use parentheses to set priorities in the expression
string expressionExample2 = "((2 + 5) * 2) * 3";

// You can also represent numbers like this
string expressionExample3 = "1E+3";

// This is an expression with two operators and two functions
string expressionExample4 = "fact(2 + 3) * fact3";

// You can also use implicit multiplication
string expressionExample5 = "2(5)";
```

## Available operators

### Arithmetic

| Operator | Description                                | Examples             |
|----------|--------------------------------------------|----------------------|
| Add      | Sums two numeric values                    | `x + y` or `x add y` |
| Subtract | Subtracts one numerical value from another | `x - y` or `x sub y` |
| Multiply | Multiplies two values                      | `x * y` or `x mul y` |
| Divide   | Divide one number by another               | `x / y` or `x div y` |
| Power    | Powers one number by another               | `x ^ y` or `x pow y` |
| Root     | Calculates the x root of y                 | `x √ y` or `x rt y`  |
| Modulus  | Returns the remainder of a division        | `x % y` or `x mod y` |

### Logical and Comparison

| Operator      | Description                                                             | Examples   |
|---------------|-------------------------------------------------------------------------|------------|
| And           | Returns true if both values are true, otherwise false                   | `x && y`   |
| Or            | Returns true if any of the values are true                              | `x \|\| y` |
| Equal         | Returns true if the two values have the same value                      | `x == y`   |
| Not Equal     | Returns true if the two values are different                            | `x != y`   |
| Greater       | Returns true if the first number is greater than the second             | `x > y`    |
| Greater Equal | Returns true if the first number is greater than or equal to the second | `x >= y`   |
| Less          | Returns true if the first number is less than the second                | `x < y`    |
| Less Equal    | Returns true if the first number is less than or equal to the second    | `x <= y`   |

### Special

| Operator  | Description                                                                             | Examples    |
|-----------|-----------------------------------------------------------------------------------------|-------------|
| Semicolon | Does not return any value, it can only be used in functions to pass multiple parameters | `fun(x; y)` |

## Available functions

### Arithmetic

| Function            | Description                                         | Examples          | Parameter limit          |
|---------------------|-----------------------------------------------------|-------------------|--------------------------|
| Add                 | Sum the numerical values                            | `add(x; y; ...)`  | Min: 1 \| Max: Undefined |
| Factorial           | Calculates the factorial of a number                | `fact(x)`         | Min: 1 \| Max: 1         |
| Min                 | Returns the smallest value passed into the function | `min(x; y; ...)`  | Min: 1 \| Max: Undefined |
| Max                 | Returns the largest value passed into the function  | `max(x; y; ...)`  | Min: 1 \| Max: Undefined |
| Square root         | Returns the square root of x                        | `√x` or `sqrt(x)` | Min: 1 \| Max: 1         |
| Cube root           | Returns the cube root of x                          | `cbrt(x)`         | Min: 1 \| Max: 1         |
| Logarithm (base y)  | Calculates the logarithm of x (base y)              | `log(x; y)`       | Min: 2 \| Max: 2         |
| Logarithm (base 10) | Calculates the logarithm of x (base 10)             | `log(x)`          | Min: 1 \| Max: 1         |
| Ceiling             | Returns the ceiling of a number                     | `ceil(x)`         | Min: 1 \| Max: 1         |
| Floor               | Returns the floor of a number                       | `floor(x)`        | Min: 1 \| Max: 1         |

### Logic and Comparison

| Function | Description                                                | Examples       | Parameter limit  |
|----------|------------------------------------------------------------|----------------|------------------|
| Not      | Inverts the boolean value                                  | `!x`           | Min: 1 \| Max: 1 |
| If       | Check the x condition. True returns the y, otherwise the z | `if(x; y; z)`  | Min: 3 \| Max: 3 |

### Trigonometry

| Function           | Description                             | Examples  | Parameter limit  |
|--------------------|-----------------------------------------|-----------|------------------|
| Cosine             | Gets the cosine of a number             | `cos(x)`  | Min: 1 \| Max: 1 |
| Sine               | Gets the sine of a number               | `sin(x)`  | Min: 1 \| Max: 1 |
| Tangent            | Gets the tangent of a number            | `tan(x)`  | Min: 1 \| Max: 1 |
| Cosine Hyperbolic  | Gets the cosine hyperbolic of a number  | `cosh(x)` | Min: 1 \| Max: 1 |
| Sine Hyperbolic    | Gets the sine hyperbolic of a number    | `sinh(x)` | Min: 1 \| Max: 1 |
| Tangent Hyperbolic | Gets the tangent hyperbolic of a number | `tanh(x)` | Min: 1 \| Max: 1 |
| Radian             | Convert degrees to radians              | `rad(x)`  | Min: 1 \| Max: 1 |
| Degree             | Convert radians to degrees              | `deg(x)`  | Min: 1 \| Max: 1 |

### Others

| Function | Description                                            | Examples                      | Parameter limit  |
|----------|--------------------------------------------------------|-------------------------------|------------------|
| Random   | Same functionality as Random.Next                      | `random(x)` or `random(x; y)` | Min: 1 \| Max: 2 |

## Available variables

### Number

| Variable     | Description                   | Examples          |
|--------------|-------------------------------|-------------------|
| Pi           | Represents the PI value       | `π` or `pi`       |
| Euler        | Represents the Euler value    | `e` or `euler`    |
| Infinity     | Represents the Infinity value | `∞` or `infinity` |
| Not a number | Represents the 'Not a number' | `NaN`             |

### Boolean

| Variable | Description                  | Examples |
|----------|------------------------------|----------|
| True     | Represents the boolean true  | `true`   |
| False    | Represents the boolean false | `false`  |

## Building your own functions and variables

Currently the classes you can inherit are: FunctionItem, NumberItem and BooleanItem

Remember that you need to provide the assembly where the custom functions and variables are located

Obs: Currently the library does not support ServiceProvider to construct functions and variables, I don't know when I
will implement it, if you want, feel free to send a pull request!

### Example

```csharp
internal sealed class CustomVariable : NumberItem
{
    // You can add multiple strings
    public CustomVariable(): base("MYCUSTOMVARIABLE")
    {
    }
    
    /*
    * If a variable is constant or a function is pure. You can set this 
    * property to true, this will optimize a calculator that uses the 
    * option to cache expressions
    */
    public override bool Pure => true; // Default is false
    
    public override double GetValue()
    {
        return 10;
    }
}

internal sealed class CustomFunction : FunctionItem
{
    public CustomFunction(): base("?")
    {
    }
    
    public override int ParametersMin => 1; // Default is 1
    
    public override int ParametersMax => 1; // Default is -1 (No limits)
    
    public override bool Pure => true;
    
    public override Variable Operate(ReadOnlySpan<Variable> variables)
    {
        return 2 * variables[0].Double; // The struct Variable already does the implicit conversion
    }
}

public class Program
{
    public static void Main()
    {
        Type type = typeof(CustomVariable);
        ICalculator calculator = new CalculatorBuilder()
            .WithAssemblies(type.Assembly)
            .Build();

        string expression = "?MyCustomVariable + 10";
        var mathResult = calculator.GetDoubleResult(expression);

        Console.WriteLine(mathResult.Result); // 30
    }
}
```

## Benchmark (100,000 iterations)

In the Benchmark below, **100,000 iterations** were made for each method

``` ini

BenchmarkDotNet v0.13.12, Arch Linux
AMD Ryzen 5 3500X, 1 CPU, 6 logical and 6 physical cores
.NET SDK 8.0.102
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2


```
| Method                       | Options          | Mean       | Error     | StdDev    | Allocated |
|----------------------------- |----------------- |-----------:|----------:|----------:|----------:|
| NumberOnly                   | Default          |   7.873 ms | 0.0076 ms | 0.0064 ms |      12 B |
| Random                       | Default          |  38.121 ms | 0.1426 ms | 0.1334 ms |      53 B |
| SimpleCalculation            | Default          |  23.906 ms | 0.1208 ms | 0.1130 ms |      23 B |
| EulerPlusEulerPlusEuler      | Default          |  33.412 ms | 0.0941 ms | 0.0735 ms |      49 B |
| ComplexCalculation           | Default          |  63.722 ms | 0.0783 ms | 0.0733 ms |      92 B |
| HeavyCalculation             | Default          | 633.367 ms | 5.0311 ms | 4.7061 ms |     736 B |
| HeavyCalculationNoWhiteSpace | Default          | 622.123 ms | 0.6197 ms | 0.5797 ms |     736 B |
| ManyParenthesesCalculation   | Default          | 119.524 ms | 2.2572 ms | 2.4151 ms |     147 B |
| FactorialCalculation         | Default          |  77.468 ms | 0.3160 ms | 0.2956 ms |     105 B |
| SquareRootStringCalculation  | Default          |  18.776 ms | 0.0658 ms | 0.0616 ms |      23 B |
| SquareRootCharCalculation    | Default          |  13.621 ms | 0.0721 ms | 0.0674 ms |      12 B |
| AddFunctionCalculation       | Default          |  34.724 ms | 0.0325 ms | 0.0288 ms |      49 B |
| AddOperationCalculation      | Default          |  23.851 ms | 0.0384 ms | 0.0360 ms |      23 B |
| NumberOnly                   | CacheExpressions |   7.273 ms | 0.0595 ms | 0.0557 ms |       6 B |
| Random                       | CacheExpressions |  22.307 ms | 0.0606 ms | 0.0538 ms |      23 B |
| SimpleCalculation            | CacheExpressions |   9.341 ms | 0.0842 ms | 0.0746 ms |      12 B |
| EulerPlusEulerPlusEuler      | CacheExpressions |  11.500 ms | 0.0525 ms | 0.0438 ms |      12 B |
| ComplexCalculation           | CacheExpressions |  12.127 ms | 0.0457 ms | 0.0405 ms |      12 B |
| HeavyCalculation             | CacheExpressions |  61.943 ms | 0.7462 ms | 0.6980 ms |      82 B |
| HeavyCalculationNoWhiteSpace | CacheExpressions |  61.483 ms | 0.3993 ms | 0.3539 ms |      82 B |
| ManyParenthesesCalculation   | CacheExpressions |  75.954 ms | 0.5798 ms | 0.5140 ms |     105 B |
| FactorialCalculation         | CacheExpressions |  14.111 ms | 0.0395 ms | 0.0350 ms |      12 B |
| SquareRootStringCalculation  | CacheExpressions |   7.658 ms | 0.0664 ms | 0.0621 ms |       6 B |
| SquareRootCharCalculation    | CacheExpressions |   6.741 ms | 0.0239 ms | 0.0224 ms |       6 B |
| AddFunctionCalculation       | CacheExpressions |   8.907 ms | 0.0379 ms | 0.0354 ms |      12 B |
| AddOperationCalculation      | CacheExpressions |   7.472 ms | 0.0289 ms | 0.0270 ms |       6 B |

### Expressions

| Name                         | Expression                                                                                                                                                                                                                                                                  |
|------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| NumberOnly                   | 3                                                                                                                                                                                                                                                                           |
| Random                       | random(1; 10)                                                                                                                                                                                                                                                               |
| SimpleCalculation            | 2 + 5 * 6                                                                                                                                                                                                                                                                   |
| EulerPlusEulerPlusEuler      | EULER + EULER + EULER                                                                                                                                                                                                                                                       |
| ComplexCalculation           | 2 ^ 2 + (4 + 5 * (2 √ 9))                                                                                                                                                                                                                                                   |
| HeavyCalculation             | (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) / (2 ^ 2 + (4 + 5 * (2 √ 9))) |
| HeavyCalculationNoWhiteSpace | (2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))/(2^2+(4+5*(2√9)))                                                                                                           |
| ManyParenthesesCalculation   | ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((2 + 2))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))                                                               |
| FactorialCalculation         | fact2 + (fact2 + fact(fact2 + 2))                                                                                                                                                                                                                                           |
| SquareRootStringCalculation  | SQRT9                                                                                                                                                                                                                                                                       |
| SquareRootCharCalculation    | √9                                                                                                                                                                                                                                                                          |
| AddFunctionCalculation       | ADD(1;2;3)                                                                                                                                                                                                                                                                  |
| AddOperationCalculation      | 1+2+3                                                                                                                                                                                                                                                                       |

## Future goals

- Add more functions, variables and operators
- Try to increase performance


## Feedback and Bugs

If you find any bugs or have any suggestions, feel free to send them!