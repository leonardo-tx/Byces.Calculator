using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    /// <summary>
    /// The class that represents a variable of an expression.
    /// </summary>
    public readonly struct Variable
    {
        private Variable(double number, bool boolean, VariableType type)
        {
            Number = number;
            Boolean = boolean;
            Type = type;
        }

        internal readonly double Number;

        internal readonly bool Boolean;

        /// <summary>
        /// The type of the current variable.
        /// </summary>
        public VariableType Type { get; }

        /// <summary>
        /// Gets the value stored in this instance as a <see cref="double"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public double Double 
        {
            get
            {
                return Type switch
                {
                    VariableType.Number => Number,
                    _ => throw new InvalidArgumentExpressionException(),
                };
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as a <see cref="float"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public float Float
        {
            get
            {
                switch (Type)
                {
                    case VariableType.Number:
                        if (Number > float.MaxValue || Number < float.MinValue) throw new InvalidArgumentExpressionException();
                        return (float)Number;
                    default:
                        throw new InvalidArgumentExpressionException();
                }
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as a <see cref="long"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public long Long
        {
            get
            {
                if (Type != VariableType.Number) throw new InvalidArgumentExpressionException();
                double diff = Number - Math.Floor(Number);
                
                if (diff != 0 || Number > long.MaxValue || Number < long.MinValue) throw new InvalidArgumentExpressionException();
                return (long)Number;
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as an <see cref="int"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public int Int
        {
            get
            {
                if (Type != VariableType.Number) throw new InvalidArgumentExpressionException();
                double diff = Number - Math.Floor(Number);

                if (diff != 0 || Number > int.MaxValue || Number < int.MinValue) throw new InvalidArgumentExpressionException();
                return (int)Number;
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as a <see cref="bool"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public bool Bool
        {
            get
            {
                if (Type != VariableType.Boolean) throw new InvalidArgumentExpressionException();
                return Boolean;
            }
        }

        /// <summary>
        /// Implicitly converts a <see cref="double" /> to a <see cref="Variable" />.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The representation of <see cref="double" /> as a <see cref="Variable" />.</returns>
        public static implicit operator Variable(double number) => new(number, false, VariableType.Number);

        /// <summary>
        /// Implicitly converts a <see cref="bool" /> to a <see cref="Variable" />.
        /// </summary>
        /// <param name="boolean">The boolean to convert.</param>
        /// <returns>The representation of <see cref="bool" /> as a <see cref="Variable" />.</returns>
        public static implicit operator Variable(bool boolean) => new(0, boolean, VariableType.Boolean);
    }
}