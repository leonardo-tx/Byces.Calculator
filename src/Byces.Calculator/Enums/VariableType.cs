using System;

namespace Byces.Calculator.Enums
{
    internal abstract class VariableType : ExpressionType<VariableType>
    {
        internal virtual double GetNumber() => throw new NotSupportedException();

        internal virtual bool GetBoolean() => throw new NotSupportedException();
    }
}