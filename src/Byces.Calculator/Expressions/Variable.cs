using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Variable
    {
        private Variable(double number, bool boolean, VariableType type)
        {
            Number = number;
            Boolean = boolean;
            Type = type;
        }

        internal readonly double Number;

        internal readonly bool Boolean;

        public VariableType Type { get; }

        /// <summary>
        /// Gets the value stored in this instance as a <see langword="double"/>.
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
        /// Gets the value stored in this instance as a <see langword="float"/>.
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
        /// Gets the value stored in this instance as a <see langword="long"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public long Long
        {
            get
            {
                if (Type != VariableType.Number) throw new InvalidArgumentExpressionException();
                double diff = Number - Math.Floor(Number);
                
                if (diff != 0 || Number > long.MaxValue || Number < long.MinValue) throw new InvalidArgumentExpressionException();
                long result = (long)Number;

                if (result < 0 != Number < 0) return result;
                return result;
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as an <see langword="int"/>.
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
        /// Gets the value stored in this instance as a <see langword="bool"/>.
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

        public static implicit operator Variable(double number) => new Variable(number, false, VariableType.Number);

        public static implicit operator Variable(bool boolean) => new Variable(0, boolean, VariableType.Boolean);
    }
}