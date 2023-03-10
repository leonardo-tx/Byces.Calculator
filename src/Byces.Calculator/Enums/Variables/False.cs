namespace Byces.Calculator.Enums.Variables
{
    internal sealed class False : VariableRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override string StringRepresentation => "FALSE";

        internal override bool GetBoolean() => false;
    }
}