using System;
using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;

namespace Byces.Calculator.Representations
{
    internal abstract class OperatorRepresentation : ExpressionRepresentation<OperatorRepresentation>
    {
        protected OperatorRepresentation() : base(ExpressionConflict.Operator, ExpressionConflict.Operator) { }

        internal abstract OperatorPriority Priority { get; }

        internal abstract Variable Operate(Variable left, Variable right);
        
        internal static OperatorRepresentation? FindOperator(ContentBuilder contentBuilder, ReadOnlySpan<char> expressionSpan)
        {
            if (contentBuilder.LastIndex == expressionSpan.Length) return null;

            ReadOnlySpan<char> currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + 1)];
            if (currentSpan.Length > MaxStringSize || !TryParse(currentSpan, out OperatorRepresentation? firstResult)) return null;
            
            int increaseLastIndex = 0, foundLength = currentSpan.Length;
            OperatorRepresentation? value = null;
            
            for (int i = 0; i < firstResult.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = firstResult.RepresentableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1;
                while (j <= representableConflict.Difference && contentBuilder.LastIndex + j < expressionSpan.Length) j++;
                
                currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + --j + 1)];

                bool parseResult = TryParse(currentSpan, out OperatorRepresentation? value2);
                if (!parseResult || increaseLastIndex > j) continue;
                
                increaseLastIndex = j;
                value = value2;
            }
            if (value == null) return firstResult;
            
            contentBuilder.LastIndex += increaseLastIndex;
            return value;
        }
    }
}