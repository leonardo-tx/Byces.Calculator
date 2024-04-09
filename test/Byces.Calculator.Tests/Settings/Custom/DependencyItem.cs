using Byces.Calculator.Expressions.Items.Variables;
using Byces.Calculator.Tests.Settings.Dependencies;

namespace Byces.Calculator.Tests.Settings.Custom;

public sealed class DependencyItem : BooleanItem
{
    public DependencyItem(TestDependency dependency): base("DEPENDENCY")
    {
        _dependency = dependency;
    }

    private readonly TestDependency _dependency;

    public override bool GetValue()
    {
        return _dependency.Path == null;
    }
}