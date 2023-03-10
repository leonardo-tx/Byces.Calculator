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
```

## Available operations

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
| Not Equal     | Returns true if the two values are differen                             | `x != y`   |
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

### Logic and Comparison

| Function | Description               | Examples | Parameter limit  |
|----------|---------------------------|----------|------------------|
| Not      | Inverts the boolean value | `!x`     | Min: 1 \| Max: 1 |

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

## Future goals

- Be able to create your own functions and variables in a simple way
- Add more functions, variables and operators
- Try to increase performance


## Feedback and Bugs

If you find any bugs or have any suggestions, feel free to send them :D !