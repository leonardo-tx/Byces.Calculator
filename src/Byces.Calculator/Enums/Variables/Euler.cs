using System;

namespace Byces.Calculator.Enums.Variables
{
    internal sealed class Euler : VariableType
    {
        public override ResultType ResultType => ResultType.Number;
        protected override string StringRepresentation => "EULER";

        internal override double GetNumber() => Math.E;
    }
}