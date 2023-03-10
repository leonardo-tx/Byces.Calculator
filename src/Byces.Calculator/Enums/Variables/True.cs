namespace Byces.Calculator.Enums.Variables
{
    internal sealed class True : VariableRepresentation
    {
        public override ResultType ResultType => ResultType.Boolean;
        public override string StringRepresentation => "TRUE";

        internal override bool GetBoolean() => true;
    }
}