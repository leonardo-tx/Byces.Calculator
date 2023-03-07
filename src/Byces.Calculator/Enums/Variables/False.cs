namespace Byces.Calculator.Enums.Variables
{
    internal sealed class False : VariableType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override string StringRepresentation => "FALSE";

        internal override bool GetBoolean() => false;
    }
}