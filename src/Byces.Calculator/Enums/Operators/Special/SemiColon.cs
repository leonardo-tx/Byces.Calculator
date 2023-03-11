namespace Byces.Calculator.Enums.Operators.Special
{
    internal sealed class SemiColon : OperatorRepresentation
    {
        public override char CharRepresentation => ';';
        internal override OperatorPriority Priority => OperatorPriority.SemiColon;
    }
}