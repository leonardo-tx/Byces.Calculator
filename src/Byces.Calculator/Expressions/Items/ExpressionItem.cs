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
        /// The character limit that a <see cref="StringRepresentation"/> can have.
        /// </summary>
        public const int StringSizeLimit = 128;

        /// <summary>
        /// Initializes a new <see cref="ExpressionItem{T}" /> class.
        /// </summary>
        protected ExpressionItem()
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();
            bool charIsDefault = CharRepresentation == '\0';

            if (stringIsDefault && charIsDefault)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has no representation");
            if (!charIsDefault && (char.IsWhiteSpace(CharRepresentation) || char.IsDigit(CharRepresentation) || CharRepresentation == '(' || CharRepresentation == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a char representation with an illegal character.");
            if (!stringIsDefault && spanRepresentation.Length == 1)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with length 1.");
            if (!stringIsDefault && spanRepresentation.Length > StringSizeLimit)
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation above the allowed limit of {StringSizeLimit}.");
            if (!stringIsDefault && spanRepresentation.Any(x => char.IsWhiteSpace(x) || char.IsDigit(x) || x == '(' || x == ')'))
                throw new Exception($"Unable to initialize the type. The {GetType().FullName} class has a string representation with illegal characters.");
        }
        
        internal Conflict[] RepresentableConflicts = Array.Empty<Conflict>();

        /// <summary>
        /// The string representation of the specific expression item. The value needs to be constant.
        /// Default is <see cref="string.Empty" />
        /// </summary>
        public virtual string StringRepresentation => string.Empty;

        /// <summary>
        /// The char representation of the specific expression item. The value needs to be constant.
        /// </summary>
        public virtual char CharRepresentation => '\0';
    }
}