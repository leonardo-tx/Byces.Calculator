namespace Byces.Calculator.Tests.Settings.Dependencies;

public sealed class TestDependency
{
    public TestDependency(string? path)
    {
        Path = path;
    }
    
    public string? Path { get; }
}