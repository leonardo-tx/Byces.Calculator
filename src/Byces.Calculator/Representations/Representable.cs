using System;

namespace Byces.Calculator.Representations
{
    internal abstract class Representable
    {
        protected Representable(int value)
        {
            Value = value;
        }

        internal Conflict[] RepresentableConflicts = Array.Empty<Conflict>();

        internal int Value { get; }

        public virtual string StringRepresentation => string.Empty;

        public virtual char CharRepresentation => '\0';
    }
}