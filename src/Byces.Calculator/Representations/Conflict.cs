using Byces.Calculator.Enums;

namespace Byces.Calculator.Representations
{
    internal readonly struct Conflict
    {
        internal Conflict(int diff, ExpressionConflict expressionConflict, RepresentableType representableType)
        {
            Difference = diff;
            ExpressionConflict = expressionConflict;
            Representable = representableType;
        }

        internal int Difference { get; }

        internal ExpressionConflict ExpressionConflict { get; }

        internal RepresentableType Representable { get; }
    }
}