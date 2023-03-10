namespace Byces.Calculator.Enums.Operations
{
    internal class SemiColon : OperatorRepresentation
    {
        public override ResultType ResultType => ResultType.Undefined;
        public override char CharRepresentation => ';';
        internal override OperatorPriority Priority => OperatorPriority.SemiColon;
    }
}