using System;

namespace Byces.Calculator.Representations
{
    internal abstract class Representable
    {
        internal Conflict[] RepresentableConflicts = Array.Empty<Conflict>();

        public virtual string StringRepresentation => string.Empty;

        public virtual char CharRepresentation => '\0';
    }
}