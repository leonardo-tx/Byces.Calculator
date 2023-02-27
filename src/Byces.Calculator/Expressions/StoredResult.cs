namespace Byces.Calculator.Expressions
{
    internal sealed class StoredResult
    {
        public StoredResult()
        {
            Expression = string.Empty;
            Result = default!;
        }

        internal string Expression { get; set; }

        internal double Result { get; set; }
    }
}