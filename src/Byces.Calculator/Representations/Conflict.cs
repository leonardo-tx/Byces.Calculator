using Byces.Calculator.Enums;

namespace Byces.Calculator.Representations
{
    internal readonly struct Conflict
    {
        internal Conflict(int diff, RepresentableType representableType)
        {
            Difference = diff;
            Representable = representableType;
        }

        internal int Difference { get; }

        internal RepresentableType Representable { get; }
    }
}