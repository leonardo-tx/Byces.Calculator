using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Value
    {
        internal Value(double number, bool boolean, ResultType resultType)
        {
            _number = number;
            _boolean = boolean;
            ResultType = resultType;
        }

        private readonly double _number;

        private readonly bool _boolean;

        internal double Number 
        {
            get
            {
                if (ResultType != ResultType.Number) throw new InvalidArgumentExpressionException();
                return _number;
            }
        }

        internal bool Boolean
        {
            get
            {
                if (ResultType != ResultType.Boolean) throw new InvalidArgumentExpressionException();
                return _boolean;
            }
        }

        internal ResultType ResultType { get; }

        public static implicit operator Value(double number) => new Value(number, false, ResultType.Number);

        public static implicit operator Value(bool boolean) => new Value(0, boolean, ResultType.Boolean);
    }
}