using Byces.Calculator.Extensions;
using System;

namespace Byces.Calculator.Expressions.Items
{
    /// <summary>
    /// The class that represents a expression item on a <see cref="Calculator"/>.
    /// </summary>
    public abstract class ExpressionItem<T> where T : ExpressionItem<T>
    {
        /// <summary>
        /// The character limit that a string representation can have.
        /// </summary>
        public const int StringSizeLimit = 128;

        /// <summary>
        /// Initializes a new <see cref="ExpressionItem{T}" /> class.
        /// </summary>
        protected ExpressionItem(params string[] stringRepresentations)
        {
            foreach (string item in stringRepresentations)
            {
                ReadOnlySpan<char> s = item;

                if (s.IsEmpty || s.IsWhiteSpace())
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has an empty representation.");
                if (s.Length > StringSizeLimit)
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation above the allowed limit of {StringSizeLimit}.");
                if (s.Any(x => char.IsWhiteSpace(x) || char.IsDigit(x) || x == '(' || x == ')'))
                    throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with illegal characters.");
            }
            StringRepresentations = stringRepresentations;
        }
        
        internal Conflict[] RepresentableConflicts = Array.Empty<Conflict>();

        internal string[] StringRepresentations { get; private init; }
    }
}