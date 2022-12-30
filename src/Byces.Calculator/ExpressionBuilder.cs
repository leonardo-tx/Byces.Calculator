using System;
using System.Collections.Generic;
using Byces.Calculator.Enums;

namespace Byces.Calculator
{
    /// <summary>
    /// This class provides the direct building of an <see cref="Byces.Calculator.Expression"/> in a simplified way to be used in the <see cref="Byces.Calculator"/>.
    /// </summary>
    public sealed class ExpressionBuilder
    {
        /// <summary>
        /// Initializes a new <see cref="ExpressionBuilder"/> class.
        /// </summary>
        public ExpressionBuilder()
        {
            Expression = string.Empty;
        }

        private const string AllOperators = "+-*/^√";
        private const string AllValidNumbers = "0123456789.,()";

        /// <summary>
        /// Gets or sets the expression of a <see cref="ExpressionBuilder"/>.
        /// </summary>
        /// <returns>The stored expression of the builder, or <see cref="string.Empty"/> if none is set.</returns>
        public string Expression { get; set; }

        /// <summary>
        /// Sets the expression to be builded.
        /// </summary>
        public ExpressionBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="Byces.Calculator.Expression"/> to be used in <see cref="Byces.Calculator"/>.
        /// </summary>
        /// <returns>The built calculator result.</returns>
        public Expression Build()
        {
            if (string.IsNullOrWhiteSpace(Expression)) return Byces.Calculator.Expression.Empty;
            try
            {
                (IList<Operation> operatorsList, IList<double> numbersList, IList<int> prioritiesList) = FormatExpression();
                return new Expression(numbersList, operatorsList, prioritiesList, true, null);
            }
            catch (Exception ex)
            {
                return new Expression(new List<double>(0), new List<Operation>(0), new List<int>(0), false, ex.Message);
            }
        }

        private (IList<Operation>, IList<double>, IList<int>) FormatExpression()
        {
            ReadOnlySpan<char> expressionSpan = Expression.Replace(" ", "");
            if (!IsContainersValid(expressionSpan)) throw new ArgumentException("The provided expression is missing containers to be closed.");
            
            int capacity = CountAll(expressionSpan, AllOperators);
            
            if ((2 * capacity + 1) % 2 == 0) throw new ArgumentException("The provided expression is not complete");
            IList<double> numbersList = new List<double>(capacity + 1);
            IList<Operation> operatorsList = new List<Operation>(capacity);
            IList<int> prioritiesList = new List<int>(capacity);

            ConfigureLists(expressionSpan, numbersList, operatorsList, prioritiesList);
            return (operatorsList, numbersList, prioritiesList);
        }

        private static bool IsContainersValid(ReadOnlySpan<char> expressionSpan)
        {
            int unclosedParentheses = 0;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                switch (expressionSpan[i])
                {
                    case '(':
                        unclosedParentheses++;
                        break;
                    case ')':
                        if (unclosedParentheses-- == 0) throw new ArgumentException("Provided expression has misplaced parentheses.");
                        break;
                }
            }
            return unclosedParentheses == 0;
        }

        private static int CountAll(ReadOnlySpan<char> expressionSpan, ReadOnlySpan<char> characters)
        {
            int count = 0;
            for (int i = 0; i < expressionSpan.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (expressionSpan[i] == characters[j]) count++;
                }
            }
            return count;
        }

        private static void ConfigureLists(ReadOnlySpan<char> expressionSpan, IList<double> numbersList, IList<Operation> operatorsList, IList<int> prioritiesList)
        {
            for (int i = 0, numberFirstIndex = 0, priority = 0; i < expressionSpan.Length; i++)
            {
                if (!AllValidNumbers.Contains(expressionSpan[i]))
                {
                    if (expressionSpan[i] == '-' && (i == 0 || AllOperators.Contains(expressionSpan[i - 1]))) continue;
                    if (!AllOperators.Contains(expressionSpan[i])) throw new ArgumentException("Provided expression has a unknown operator");

                    var slice1 = GetValidNumberSlice(expressionSpan, numberFirstIndex, i);
                    if (!double.TryParse(slice1, out _)) throw new ArgumentException("Provided expression has a number that could not be parsed");

                    numbersList.Add(double.Parse(slice1));
                    operatorsList.Add(GetOperation(expressionSpan[i]));
                    prioritiesList.Add(priority);

                    numberFirstIndex = i + 1;
                    continue;
                }
                if (expressionSpan[i] == '(') priority++;
                else if (expressionSpan[i] == ')') priority--;
                if (expressionSpan.Length != i + 1) continue;

                var slice2 = GetValidNumberSlice(expressionSpan, numberFirstIndex, i + 1);
                if (!double.TryParse(slice2, out _)) throw new ArgumentException("Provided expression has a number that could not be parsed");
                numbersList.Add(double.Parse(slice2));
            }
        }

        private static ReadOnlySpan<char> GetValidNumberSlice(ReadOnlySpan<char> expressionSpan, int firstIndex, int lastIndex)
        {
            var slice = expressionSpan[firstIndex..lastIndex];
            for (int i = 0; i < slice.Length; i++)
            {
                switch (slice[i])
                {
                    case '(':
                        firstIndex++;
                        break;
                    case ')':
                        lastIndex--;
                        break;
                }
            }
            return expressionSpan[firstIndex..lastIndex];
        }

        private static Operation GetOperation(char character)
        {
            return character switch
            {
                '√' => Operation.Root,
                '^' => Operation.Power,
                '*' => Operation.Multiply,
                '/' => Operation.Divide,
                '+' => Operation.Add,
                '-' => Operation.Subtract,
                _ => throw new ArgumentException("Invalid operator."),
            };
        }
    }
}