using Byces.Calculator.Enums;

namespace Byces.Calculator.Representations
{
    internal readonly struct Conflict
    {
        internal Conflict(int value, int diff, ExpressionConflict expressionConflict, RepresentableType representableType)
        {
            Value = value;
            Difference = diff;
            ExpressionConflict = expressionConflict;
            Representable = representableType;
        }

        internal int Value { get; }

        internal int Difference { get; }

        internal ExpressionConflict ExpressionConflict { get; }

        internal RepresentableType Representable { get; }
    }
}