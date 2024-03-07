using System;
using Byces.Calculator.Expressions.Items.Variables;

namespace Byces.Calculator.Tests.Settings.Custom;

public class QuestionMarkItem : BooleanItem
{
    public QuestionMarkItem(): base("?")
    {
    }

    public override bool GetValue()
    {
        return Random.Shared.Next() % 2 == 0;
    }
}