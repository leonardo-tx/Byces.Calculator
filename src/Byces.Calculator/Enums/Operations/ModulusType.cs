namespace Byces.Calculator.Enums.Operations
{
    internal sealed class ModulusType : OperationType
    {
        protected override int Value => 4;
        internal override string StringRepresentation => "MOD";
        internal override char CharRepresentation => '%';

        internal override double Operate(double firstNumber, double secondNumber)
        {
            if (secondNumber == 0) return firstNumber;
            if (firstNumber < 0 && secondNumber > 0)
            {
                double result = secondNumber - firstNumber * -1 % secondNumber;
                if (result == secondNumber) return 0;
                
                return result;
            }
            if (firstNumber > 0 && secondNumber < 0)
            {
                double result = secondNumber - firstNumber % secondNumber * -1;
                if (result == secondNumber) return 0;

                return result;
            }
            return firstNumber % secondNumber;
        }
    }
}