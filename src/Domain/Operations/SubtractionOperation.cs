using Domain.Enums;

namespace Domain.Operations
{
    public class SubtractionOperation : BaseOperation
    {
        public SubtractionOperation() : base('-', OperationPriority.Low){}
        public override double Calculate(double firstNum, double secondNum) => firstNum - secondNum;
    }
}
