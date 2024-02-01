using Byces.Calculator.Enums;
using Byces.Calculator.Expressions;
using System;

namespace Byces.Calculator.Representations
{
    internal abstract class FunctionRepresentation : ExpressionRepresentation<FunctionRepresentation>
    {
        protected FunctionRepresentation() : base(ExpressionConflict.Function | ExpressionConflict.Variable, ExpressionConflict.Function)
        {
            ReadOnlySpan<char> spanRepresentation = StringRepresentation;
            bool stringIsDefault = spanRepresentation.IsEmpty || spanRepresentation.IsWhiteSpace();

            if (!stringIsDefault && (spanRepresentation.StartsWith("+") || spanRepresentation.StartsWith("-")))
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} has a string representation with illegal characters.");
            if (ParametersMax == 0)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} cannot have a parameter limit of 0");
            if (ParametersMin < 1)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} cannot have a minimum parameter less than 1");
            if (ParametersMax > 0 && ParametersMin > ParametersMax)
                throw new Exception($"Could not initialize the function. The class {GetType().FullName} has a maximum parameter limit '{ParametersMax}' less than the minimum '{ParametersMin}'");
        }

        public virtual int ParametersMin => 1;

        public virtual int ParametersMax => -1;

        public abstract Variable Operate(ReadOnlySpan<Variable> variables);
        
        internal static FunctionRepresentation? FindFunction(ContentBuilder contentBuilder, ReadOnlySpan<char> expressionSpan)
        {
            if (contentBuilder.LastIndex == expressionSpan.Length) return null;

            ReadOnlySpan<char> currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + 1)];
            if (currentSpan.Length > MaxStringSize || !TryParse(currentSpan, out FunctionRepresentation? firstResult)) return null;
            
            int increaseLastIndex = 0, foundLength = currentSpan.Length;
            FunctionRepresentation? value = null;
            bool parseResult = false;
            
            for (int i = 0; i < firstResult.RepresentableConflicts.Length; i++)
            {
                Conflict representableConflict = firstResult.RepresentableConflicts[i];
                if (foundLength == 1 && representableConflict.Representable != RepresentableType.Char) continue;
                if (foundLength > 1 && representableConflict.Representable != RepresentableType.String) continue;

                int j = 1;
                while (j <= representableConflict.Difference && contentBuilder.LastIndex + j < expressionSpan.Length) j++;
                
                currentSpan = expressionSpan[contentBuilder.FirstIndex..(contentBuilder.LastIndex + --j + 1)];

                FunctionRepresentation? value2 = null;
                parseResult = representableConflict.ExpressionConflict == ExpressionConflict.Variable 
                    ? VariableRepresentation.TryParse(currentSpan, out _) 
                    : TryParse(currentSpan, out value2);
                
                if (!parseResult || increaseLastIndex > j) continue;
                
                increaseLastIndex = j;
                value = value2;
            }
            if (value == null)
            {
                if (!parseResult) return firstResult;
                
                --contentBuilder.LastIndex;
                return null;
            }
            
            contentBuilder.LastIndex += increaseLastIndex;
            return value;
        }
    }
}