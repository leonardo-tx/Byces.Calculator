using System;

namespace Byces.Calculator.Enums
{
    internal abstract class Representable
    {
        protected Representable(int value)
        {
            Value = value;
        }

        internal Conflict[] representableConflicts = Array.Empty<Conflict>();

        internal int Value { get; }

        public virtual string StringRepresentation { get; } = string.Empty;

        public virtual char CharRepresentation { get; } = '\0';
    }
}