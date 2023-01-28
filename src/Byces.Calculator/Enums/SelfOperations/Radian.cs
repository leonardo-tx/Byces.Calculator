using MathNet.Numerics;

namespace Byces.Calculator.Enums.SelfOperations
{
    internal sealed class Radian : SelfOperationType
    {
        protected override int Value => 9;
        internal override string StringRepresentation => "RAD";
        internal override char CharRepresentation => default;

        internal override double Operate(double number) => number * Constants.Pi2 / 360;
    }
}