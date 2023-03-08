using Byces.Calculator.Enums;
using Byces.Calculator.Exceptions;

namespace Byces.Calculator.Expressions
{
    internal readonly struct Value
    {
        internal Value(double number, bool boolean, ResultType resultType)
        {
            this.number = number;
            this.boolean = boolean;
            ResultType = resultType;
        }

        internal readonly double number;

        internal readonly bool boolean;

        internal double Number 
        {
            get
            {
                if (ResultType != ResultType.Number) throw new InvalidArgumentExpressionException();
                return number;
            }
        }

        internal bool Boolean
        {
            get
            {
                if (ResultType != ResultType.Boolean) throw new InvalidArgumentExpressionException();
                return boolean;
            }
        }

        internal ResultType ResultType { get; }

        public static implicit operator Value(double number) => new Value(number, false, ResultType.Number);

        public static implicit operator Value(bool boolean) => new Value(0, boolean, ResultType.Boolean);
    }
}