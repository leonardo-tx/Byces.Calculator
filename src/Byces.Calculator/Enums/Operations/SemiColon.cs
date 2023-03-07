namespace Byces.Calculator.Enums.Operations
{
    internal class SemiColon : OperationType
    {
        public override ResultType ResultType => ResultType.Undefined;
        protected override char CharRepresentation => ';';
        internal override OperationPriorityType Priority => OperationPriorityType.SemiColon;
    }
}