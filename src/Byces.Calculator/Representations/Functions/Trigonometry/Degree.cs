using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations.Functions.Trigonometry
{
    internal sealed class Degree : FunctionRepresentation
    {
        public override string StringRepresentation => "DEG";
        public override int ParametersMax => 1;

        public override Variable Operate(ReadOnlySpan<Variable> variables) => variables[0].Double * 180 / Math.PI;
    }
}