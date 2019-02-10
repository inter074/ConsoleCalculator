using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Operations
{
    public abstract class BaseOperation : IOperation 
    {
        protected BaseOperation(char sign, OperationPriority priority)
        {
            Priority = priority;
            OperationSign = sign;
        }
        
        public char OperationSign { get; }

        public OperationPriority Priority { get; }

        public abstract double Calculate(double firstNum, double secondNum);
    }
}
