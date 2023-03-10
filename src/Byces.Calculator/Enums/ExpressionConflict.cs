using System;

namespace Byces.Calculator.Enums
{
    [Flags]
    internal enum ExpressionConflict
    {
        Variable = 1,
        Operator = 2,
        Function = 4,
    }
}