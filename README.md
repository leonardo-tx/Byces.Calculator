# Byces.Calculator
[![NuGet version](https://img.shields.io/nuget/v/Byces.Calculator)](https://www.nuget.org/packages/Byces.Calculator)
[![.NET](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml)

A simple .NET calculator that solves string math expressions

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
* The calculator is not sensitive to whitespace and using it has a low impact on performance

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
| If       | Check the x condition. True returns the y, otherwise the z | `if(x; y; z;)` | Min: 3 \| Max: 3 |

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

## Available variables

### Number

| Variable | Description                | Examples       |
|----------|----------------------------|----------------|
| Pi       | Represents the PI value    | `π` or `pi`    |
| Euler    | Represents the Euler value | `e` or `euler` |

### Boolean

| Variable | Description                  | Examples |
|----------|------------------------------|----------|
| True     | Represents the boolean true  | `true`   |
| False    | Represents the boolean false | `false`  |

## Public Models

### MathResult{T} (readonly struct)

| Property     | Type            | Summary                                             |
|--------------|-----------------|-----------------------------------------------------|
| IsValid      | bool            | Gets the validity of the `MathResult` instance      |
| ErrorMessage | string?         | Gets the error message of a possible syntax problem |
| Error        | ResultError     | Gets the expression error type                      |
| Result       | T               | Gets the expression result                          |

### CalculatorBuilder (class)

| Method  | Type       | Summary                            |
|---------|------------|------------------------------------|
| Build() | Calculator | Builds a new `Calculator` instance |

### Calculator (class) and ICalculator (interface)

| Method                              | Type               | Summary                                                                   |
|-------------------------------------|--------------------|---------------------------------------------------------------------------|
| GetDoubleResult(string expression)  | MathResult<double> | Gets a `MathResult<double>` calculating the given mathematical expression |
| GetBooleanResult(string expression) | MathResult<bool>   | Gets a `MathResult<bool>` calculating the given mathematical expression   |

## Benchmark

In the Benchmark below, one million iterations were made for each method

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
AMD Ryzen 5 3500X, 1 CPU, 6 logical and 6 physical cores
.NET SDK=7.0.400
  [Host]     : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2


```
|                       Method |       Mean |    Error |   StdDev | Allocated |
|----------------------------- |-----------:|---------:|---------:|----------:|
|            SimpleCalculation |   299.9 ms |  2.00 ms |  1.67 ms |     252 B |
|      EulerPlusEulerPlusEuler |   392.2 ms |  2.41 ms |  2.25 ms |     504 B |
|           ComplexCalculation |   762.7 ms |  2.46 ms |  2.30 ms |     504 B |
|             HeavyCalculation | 8,625.8 ms | 44.98 ms | 39.87 ms |     504 B |
| HeavyCalculationNoWhiteSpace | 8,489.1 ms | 23.98 ms | 21.25 ms |     504 B |
|   ManyParenthesesCalculation | 1,160.5 ms |  6.10 ms |  5.71 ms |     504 B |
|         FactorialCalculation | 1,165.7 ms |  6.35 ms |  5.94 ms |     504 B |
|  SquareRootStringCalculation |   282.7 ms |  2.65 ms |  2.48 ms |     252 B |
|    SquareRootCharCalculation |   192.0 ms |  1.19 ms |  1.00 ms |     168 B |
|       AddFunctionCalculation |   440.8 ms |  3.29 ms |  2.75 ms |     504 B |
|      AddOperationCalculation |   319.2 ms |  2.37 ms |  2.22 ms |     252 B |

### Expressions

| Name                         | Expression                                                                                                                                                                                                                                                                  |
|------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
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

- Be able to create your own functions and variables in a simple way
- Add more functions, variables and operators
- Try to increase performance


## Feedback and Bugs

If you find any bugs or have any suggestions, feel free to send them!