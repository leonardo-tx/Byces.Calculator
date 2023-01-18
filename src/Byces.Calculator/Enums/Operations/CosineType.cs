﻿using System;

namespace Byces.Calculator.Enums.Operations
{
    internal sealed class CosineType : OperationType
    {
        internal CosineType(int value) : base("Cosine", value) { }

        internal override string StringRepresentation => "COS";
        internal override char CharRepresentation => default;
        internal override OperationCategory Category => OperationCategory.Before;

        internal override double Operate(double firstNumber, double secondNumber) => throw new NotSupportedException();

        internal override double Operate(double number) => Math.Cos(number);
    }
}