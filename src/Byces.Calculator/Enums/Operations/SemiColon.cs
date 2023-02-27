using System;

namespace Byces.Calculator.Enums.Operations
{
    internal class SemiColon : OperationType
    {
        protected override int Value => 8;
        protected override string StringRepresentation => string.Empty;
        protected override char CharRepresentation => ';';
        internal override OperationPriorityType Priority => OperationPriorityType.SemiColon;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();
    }
}