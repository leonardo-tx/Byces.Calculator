namespace Byces.Calculator.Enums.Variables
{
    internal sealed class True : VariableType
    {
        public override ResultType ResultType => ResultType.Boolean;
        protected override string StringRepresentation => "TRUE";

        internal override bool GetBoolean() => true;
    }
}