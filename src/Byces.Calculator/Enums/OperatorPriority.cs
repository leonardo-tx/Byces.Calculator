using System;

namespace Byces.Calculator.Enums
{
    [Flags]
    internal enum OperatorPriority
    {
        None = 0,
        Potentiality = 1,
        Multiplicative = 2,
        Additive = 4,
        Relational = 8,
        Equality = 16,
        AndBitwise = 32,
        OrBitwise = 64,
        AndConditional = 128,
        OrConditional = 256,
        FunctionSeparator = 512
    }
}