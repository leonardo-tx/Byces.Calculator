using System;
using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Tests.Settings.Custom;

internal sealed class RandomNumberItem : NumberItem
{
    public override string StringRepresentation => "RANDOMNUMBER";

    public override double GetValue()
    {
        return Random.Shared.Next(10);
    }
}