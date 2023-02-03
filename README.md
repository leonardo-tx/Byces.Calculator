# Byces.Calculator
[![NuGet version](https://img.shields.io/nuget/v/Byces.Calculator)](https://www.nuget.org/packages/Byces.Calculator)
[![.NET](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/leonardo-tx/Byces.Calculator/actions/workflows/dotnet.yml)

A simple .NET calculator that solves string math expressions

## Usage example

```csharp
string expression = "2 + 5 * 3";

MathResult mathResult = MathResultBuilder.GetMathResult(expression);
// or
MathResult mathResult = new MathResultBuilder().WithExpression(expression).Build();

if (mathResult.IsValid) // true
{
    Console.WriteLine(mathResult.Result); // 17
}
else
{
    Console.WriteLine(mathResult.ErrorMessage);
}
```

## Public Models

### MathResult (readonly struct)

| Property     | Type            | Summary                                             |
|--------------|-----------------|-----------------------------------------------------|
| IsValid      | bool            | Gets the validity of the `MathResult` instance      |
| ErrorMessage | string?         | Gets the error message of a possible syntax problem |
| ErrorType    | ResultErrorType | Gets the expression error type                      |
| Result       | double          | Gets the expression result                          |

### MathResultBuilder (class)

| Property / Method                | Type              | Summary                                                                      |
|----------------------------------|-------------------|------------------------------------------------------------------------------|
| Expression                       | string            | Gets or sets the expression of a `MathResultBuilder`                         |
| WithExpression(expression)       | MathResultBuilder | Sets the expression to be builded                                            |
| Build()                          | MathResult        | Builds the `MathResult` with the given information                           |
| static GetMathResult(expression) | MathResult        | Gets the `MathResult` without having to create a `MathResultBuilder` object  |

## Building expressions

Before showing some syntax examples, here are some observations:

* Numbers, operations and functions are not case sensitive
* The builder is not sensitive to whitespace and using it has a low impact on performance

```csharp
// You can use signs to represent negative and positive numbers
string expressionExample1 = "-5 + +5";

// You can use parentheses to set priorities in the expression
string expressionExample2 = "((2 + 5) * 2) * 3";

// You can also represent numbers like this
string expressionExample3 = "1E+3";

// This is an expression with two operations and two functions
string expressionExample4 = "fact(2 + 3) * fact3";
```

### Available operations

#### Add

```csharp
string expression1 = "2+4+6";
string expression2 = "2 add 4 add 6";
```

#### Subtract

```csharp
string expression1 = "2-4-6";
string expression2 = "2 sub 4 sub 6";
```

#### Multiply

```csharp
string expression1 = "2*4*6";
string expression2 = "2 mul 4 mul 6";
```

#### Divide

```csharp
string expression1 = "2/4/6";
string expression2 = "2 div 4 div 6";
```

#### Power

```csharp
string expression1 = "2^4^6";
string expression2 = "2 pow 4 pow 6";
```

#### Root

```csharp
string expression1 = "2√4√6";
string expression2 = "2 rt 4 rt 6";
```

#### Modulus

```csharp
string expression1 = "2%4%6";
string expression2 = "2 mod 4 mod 6";
```

#### Log (base: firstNumber)

```csharp
string expression = "2 log 4 log 6";
```

### Available functions

#### Factorial

```csharp
string expression = "fact(3)";
```

#### Square root

```csharp
string expression1 = "sqrt(3)";
string expression2 = "√3";
```

#### Cube root

```csharp
string expression = "cbrt(3)";
```

#### Cosine

```csharp
string expression = "cos(3)";
```

#### Sine

```csharp
string expression = "sin(3)";
```

#### Tangent

```csharp
string expression = "tan(3)";
```

#### Cosine hyperbolic

```csharp
string expression = "cosh(3)";
```

#### Sine hyperbolic

```csharp
string expression = "sinh(3)";
```

#### Tangent hyperbolic

```csharp
string expression = "tanh(3)";
```

#### Radian (360º)

```csharp
string expression = "rad(3)";
```

#### Log (base 10)

```csharp
string expression = "log(3)";
```

### Available special numbers

#### PI

```csharp
string expression1 = "π";
string expression2 = "pi";
```

#### Euler

```csharp
string expression = "euler";
```

## Feedback and Bugs

Feel free to send suggestions and problems, thanks to those who read this far! :D