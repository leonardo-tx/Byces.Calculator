using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;
using System;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Variable
    {
        private Variable(double number, bool boolean, VariableType type)
        {
            _number = number;
            _boolean = boolean;
            Type = type;
        }

        internal readonly double _number;

        internal readonly bool _boolean;

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
                    VariableType.Number => _number,
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
                        if (_number > float.MaxValue || _number < float.MinValue) throw new InvalidArgumentExpressionException();
                        return (float)_number;
                }
                throw new InvalidArgumentExpressionException();
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
                double diff = _number - Math.Floor(_number);
                
                if (diff != 0 || _number > long.MaxValue || _number < long.MinValue) throw new InvalidArgumentExpressionException();
                long result = (long)_number;

                if (result < 0 != _number < 0) return result;
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
                double diff = _number - Math.Floor(_number);

                if (diff != 0 || _number > int.MaxValue || _number < int.MinValue) throw new InvalidArgumentExpressionException();
                return (int)_number;
            }
        }

        /// <summary>
        /// Gets the value stored in this instance as a <see langword="bool"/>.
        /// </summary>
        /// <exception cref="InvalidArgumentExpressionException"></exception>
        public bool Boolean
        {
            get
            {
                if (Type != VariableType.Boolean) throw new InvalidArgumentExpressionException();
                return _boolean;
            }
        }

        public static implicit operator Variable(double number) => new Variable(number, false, VariableType.Number);

        public static implicit operator Variable(bool boolean) => new Variable(0, boolean, VariableType.Boolean);
    }
}