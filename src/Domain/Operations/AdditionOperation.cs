using Domain.Enums;

namespace Domain.Operations
{
    public class AdditionOperation : BaseOperation
    {
        public AdditionOperation() : base('+', OperationPriority.Low) { }

        public override double Calculate(double firstNum, double secondNum) => firstNum + secondNum;
    }
}
