namespace Byces.Calculator.Expressions.Items
{
    internal readonly struct Conflict
    {
        internal Conflict(int diff)
        {
            Difference = diff;
        }

        internal int Difference { get; }
    }
}