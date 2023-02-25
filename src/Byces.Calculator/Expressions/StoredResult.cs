namespace Byces.Calculator.Expressions
{
    internal class StoredResult
    {
        public StoredResult()
        {
            Expression = string.Empty;
            Result = 0;
        }

        internal string Expression { get; set; }

        internal double Result { get; set; }
    }
}