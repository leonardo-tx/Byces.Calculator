using System.Collections.Generic;
using Byces.Calculator.Enums;

namespace Byces.Calculator
{
    public readonly struct Expression
    {
        internal Expression(IList<double> numbersList, IList<Operation> operatorsList, IList<int> priorities, bool isValid, string? errorMessage)
        {
            NumbersList = numbersList;
            OperatorsList = operatorsList;
            PrioritiesList = priorities;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static readonly Expression Empty = new Expression(new List<double>(1) { 0 }, new List<Operation>(0), new List<int>(0), true, null);

        public bool IsValid { get; }

        public string? ErrorMessage { get; }

        internal IList<double> NumbersList { get; }

        internal IList<Operation> OperatorsList { get; }

        internal IList<int> PrioritiesList { get; } 
    }
}