using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class Radian : FunctionRepresentation
    {
        public override string StringRepresentation => "RAD";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => variables[0].Double * Math.PI / 180;
    }
}