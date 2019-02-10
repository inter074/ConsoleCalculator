using Domain.Enums;

namespace Domain.Operations
{
    public class MultiplyOperation : BaseOperation
    {
        public MultiplyOperation() : base('*', OperationPriority.High){}
        public override double Calculate(double firstNum, double secondNum) => firstNum * secondNum;
    }
}
