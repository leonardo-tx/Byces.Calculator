using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations
{
    internal abstract class VariableRepresentation : ExpressionRepresentation<VariableRepresentation>
    {
        protected VariableRepresentation() : base(ExpressionConflict.Variable | ExpressionConflict.Function, ExpressionConflict.Variable)
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the variable. The class {GetType().FullName} has a string representation with illegal characters.");
        }

        public abstract Variable GetValue();
        
        internal static VariableRepresentation? FindVariable(ContentBuilder contentBuilder, ReadOnlySpan<char> expressionSpan)
        {
            if (contentBuilder.LastIndex == expressionSpan.Length) return null;

            ReadOnlySpan<char> currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + 1)];
            if (currentSpan.Length > MaxStringSize || !TryParse(currentSpan, out VariableRepresentation? firstResult)) return null;
            
            int increaseLastIndex = 0, foundLength = currentSpan.Length;
            VariableRepresentation? value = null;
            bool parseResult = false;
            
            for (int i = 0; i < firstResult.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = firstResult.RepresentableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1;
                while (j <= representableConflict.Difference && contentBuilder.LastIndex + j < expressionSpan.Length) j++;
                
                currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + --j + 1)];

                VariableRepresentation? value2 = null;
                parseResult = representableConflict.ExpressionConflict == ExpressionConflict.Function
                    ? FunctionRepresentation.TryParse(currentSpan, out _) 
                    : TryParse(currentSpan, out value2);
                
                if (!parseResult || increaseLastIndex > j) continue;
                
                increaseLastIndex = j;
                value = value2;
            }
            if (value == null) return parseResult ? null : firstResult;
            
            contentBuilder.LastIndex += increaseLastIndex;
            return value;
        }
    }
}