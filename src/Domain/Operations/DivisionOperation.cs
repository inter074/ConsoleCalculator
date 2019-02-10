using Domain.Enums;

namespace Domain.Operations
{
    public class DivisionOperation : BaseOperation
    {
        public DivisionOperation() : base('/', OperationPriority.High){}
        public override double Calculate(double firstNum, double secondNum) => secondNum != default(double) ? firstNum / secondNum : 0;
    }
}
