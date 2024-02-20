using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Tests.Settings.Custom;

internal sealed class TwoItem : NumberItem
{
    public override string StringRepresentation => "TWO";

    public override bool Pure => true;

    public override double GetValue() => 2;
}